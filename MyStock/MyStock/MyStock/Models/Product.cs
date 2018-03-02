using MyStock.Services;
using MyStock.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyStock.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Image { get; set; }

        public double Stock { get; set; }

        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if(string.IsNullOrEmpty(Image))
                {
                    return "ic_noimage";
                }
                return string.Format("http://productszulu.azurewebsites.net/{0}", Image.Substring(1));
            }
        }
        public int CategoryId { get; set; }
        public bool PendingToSave { get; set; }

        MessageService messageService;
        NavigationService navigationService;

        public Product()
        {
            messageService = new MessageService();
            navigationService = new NavigationService();
        }

        public ICommand EditCommand
        {
            get;
            set;
        }

        public ICommand DeleteCommand
        {
            get;
            set;
        }

        async void Edit()
        {
            MainViewModel.GetIntance().EditProduct = new EditProductViewModel(this);
            await navigationService.NavigateTo("EditProductView");
        }

        async void Delete()
        {
            var response = await messageService.ConfirmMessage("Confirm","Are you sure to delete this record?");
            if (!response)
            {
                return;
            }

            var productsViewModel = ProductsViewModel.GetIntance();
            productsViewModel.DeleteProduct(this);
        }
    }
}
