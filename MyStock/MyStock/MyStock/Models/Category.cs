using MyStock.Services;
using MyStock.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyStock.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<Product> Productos { get; set; }

        NavigationService navigationService;

        public Category()
        {
            navigationService = new NavigationService();
        }

        public ICommand SelectedCategoryCommand
        {
            get;
            set;
        }

        async void SelectedCategory()
        {
            var mainViewModel = MainViewModel.GetIntance();
            mainViewModel.Products = new ProductsViewModel(Productos);
            await navigationService.NavigateTo("ProductsView");
        }
    }
}
