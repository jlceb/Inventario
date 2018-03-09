using MyStock.Models;
using MyStock.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class NewProductViewModel : ViewModelBase
    {

        MediaFile file;

        string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                this.Notify("Description");
            }
        }

        string price;
        public string Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                this.Notify("Price");
            }
        }

        DateTime lastPurchase;
        public DateTime LastPurchase
        {
            get
            {
                return lastPurchase;
            }
            set
            {
                lastPurchase = value;
                this.Notify("LastPurchase");
            }
        }

        string stock;
        public string Stock
        {
            get
            {
                return stock;
            }
            set
            {
                stock = value;
                this.Notify("Stock");
            }
        }

        string remarks;
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                price = value;
                this.Notify("Remarks");
            }
        }

        bool isactive;
        public bool IsActive
        {
            get
            {
                return isactive;
            }
            set
            {
                isactive = value;
                this.Notify("IsActive");
            }
        }

        bool isrunning;
        public bool IsRunning
        {
            get
            {
                return isrunning;
            }
            set
            {
                isrunning = value;
                this.Notify("IsRunning");
            }
        }

        bool isenabled;
        public bool IsEnabled
        {
            get
            {
                return isenabled;
            }
            set
            {
                isenabled = value;
                this.Notify("IsEnabled");
            }
        }

        ImageSource imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                this.Notify("ImageSource");
            }
        }

        MessageService messageService;
        NavigationService navigationService;
        ApiService apiService;

        public NewProductViewModel()
        {
            apiService = new ApiService();
            messageService = new MessageService();
            navigationService = new NavigationService();

            LastPurchase = DateTime.Today;
            ImageSource = "ic_noimage";
            IsActive = true;
            IsEnabled = true;
            
        }

        public ICommand SaveCommand
        {
            get;
            set;
        }

        public ICommand ChangeImageCommand
        {
            get;
            set;
        }

        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await messageService.SendMessage("Error","You must enter a product description.");
                return;
            }

            if (string.IsNullOrEmpty(Price))
            {
                await messageService.SendMessage("Error","You must enter a product price.");
                return;
            }

            var price = decimal.Parse(Price);
            if (price < 0)
            {
                await messageService.SendMessage("Error","The price must be a value greather or equals than zero.");
                return;
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await messageService.SendMessage("Error","You must enter a product stock.");
                return;
            }

            var stock = double.Parse(Stock);
            if (stock < 0)
            {
                await messageService.SendMessage("Error","The stock must be a value greather or equals than zero.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = CodecImageService.ReadFully(file.GetStream());
                file.Dispose();
            }

            var mainViewModel = MainViewModel.GetIntance();

            var product = new Product
            {
                CategoryId = mainViewModel.Category.CategoryId,
                Description = Description,
                ImageArray = imageArray,
                IsActive = IsActive,
                LastPurchase = LastPurchase,
                Price = price,
                Remarks = Remarks,
                Stock = stock,
            };

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                product.PendingToSave = true;
                dataService.Insert(product);
                await messageService.SendMessage("Message","The product was save on local DB don't forget to " +
                    "upload the record when you have WiFi.");
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                var response = await apiService.Post(urlAPI,"/api","/Products",
                    mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken,product);

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await messageService.SendMessage("Error",response.Message);
                    return;
                }

                product = (Product)response.Result;
            }

            async void ChangeImage()
            {
                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    var source = await messageService.ShowImageOptions();

                    if (source == "Cancel")
                    {
                        file = null;
                        return;
                    }

                    if (source == "From Camera")
                    {
                        file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                Directory = "Sample",
                                Name = "test.jpg",
                                PhotoSize = PhotoSize.Small,
                            }
                        );
                    }
                    else
                    {
                        file = await CrossMedia.Current.PickPhotoAsync();
                    }
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }

                if (file != null)
                {
                    ImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                }
            }

            var productsViewModel = ProductsViewModel.GetIntance();
            productsViewModel.AddProduct(product);

            await navigationService.NavigateToBackOnMaster();

            IsRunning = false;
            IsEnabled = true;
        }
    }
}
