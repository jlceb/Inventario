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
    public class EditProductViewModel : ViewModelBase
    {
        MediaFile file;
        Product producttoedit;

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

        public EditProductViewModel(Product product)
        {
            apiService = new ApiService();
            messageService = new MessageService();
            navigationService = new NavigationService();
            this.SaveCommand = new Command(this.Save);
            this.producttoedit = product;
            Description = product.Description;
            ImageSource = product.ImageFullPath;
            Price = product.Price.ToString();
            IsActive = product.IsActive;
            LastPurchase = product.LastPurchase;
            Stock = product.Stock.ToString();
            Remarks = product.Remarks;
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
                await messageService.SendMessage("Error", "You must enter a product description.");
                return;
            }

            if (string.IsNullOrEmpty(Price))
            {
                await messageService.SendMessage("Error", "You must enter a product price.");
                return;
            }

            var price = decimal.Parse(Price);
            if (price < 0)
            {
                await messageService.SendMessage("Error", "The price must be a value greather or equals than zero.");
                return;
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await messageService.SendMessage("Error", "You must enter a product stock.");
                return;
            }

            var stock = double.Parse(Stock);
            if (stock < 0)
            {
                await messageService.SendMessage("Error", "The stock must be a value greather or equals than zero.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await messageService.SendMessage("Error", connection.Message);
                return;
            }

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = CodecImageService.ReadFully(file.GetStream());
                file.Dispose();
            }

            producttoedit.Description = Description;
            producttoedit.IsActive = IsActive;
            producttoedit.LastPurchase = LastPurchase;
            producttoedit.Price = price;
            producttoedit.Remarks = Remarks;
            producttoedit.Stock = stock;
            producttoedit.ImageArray = imageArray;

            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.Put("http://productszuluapi.azurewebsites.net", "/api", "/Products",
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken, producttoedit);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            var productsViewModel = ProductsViewModel.GetIntance();
            productsViewModel.UpdateProduct(producttoedit);
            await navigationService.NavigateToBack();

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
                  });
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

    }
}
