using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyStock.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        List<Product> listProducts;

        public ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                this.Notify("Products");
            }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                this.Notify("IsRefreshing");
            }
        }

        ApiService apiService;
        MessageService messageService;

        public ProductsViewModel(List<Product> lproducts)
        {
            instance = this;
            apiService = new ApiService();
            messageService = new MessageService();
            this.listProducts = lproducts;
            Products = new ObservableCollection<Product>(listProducts.OrderBy(x => x.Description));
        }

        public void AddProduct(Product newProduct)
        {
            IsRefreshing = true;
            listProducts.Add(newProduct);
            Products = new ObservableCollection<Product>(listProducts.OrderBy(x => x.Description));
            IsRefreshing = false;
        }

        public void UpdateProduct(Product newProduct)
        {
            IsRefreshing = true;
            var oldProduct = listProducts.Where(p => p.ProductId == newProduct.ProductId).FirstOrDefault();
            oldProduct = newProduct;
            Products = new ObservableCollection<Product>(listProducts.OrderBy(x => x.Description));
            IsRefreshing = false;
        }

        public async void DeleteProduct(Product producttodelete)
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await messageService.SendMessage("Error", connection.Message);
                return;
            }


            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.Delete("http://productszuluapi.azurewebsites.net", "/api", "/Products",
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken, producttodelete);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            listProducts.Remove(producttodelete);

            Products = new ObservableCollection<Product>(listProducts.OrderBy(x => x.Description));
            IsRefreshing = false;
        }
        //Singlenton

        static ProductsViewModel instance;

        public static ProductsViewModel GetIntance()
        {
                return instance;
        }
    }
}
