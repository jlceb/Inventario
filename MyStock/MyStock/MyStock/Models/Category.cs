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
        MessageService messageService;

        public Category()
        {
            navigationService = new NavigationService();
            messageService = new MessageService();
        }

        public override int GetHashCode()
        {
            return this.CategoryId;
        }

        public ICommand SelectedCategoryCommand
        {
            get;
            set;
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
            var mainViewModel = MainViewModel.GetIntance();
            mainViewModel.EditCategory = new EditCategoryViewModel(this);
            await navigationService.NavigateTo("EditCategoryView");
        }

        async void Delete()
        {
            var response = await messageService.ConfirmMessage("Confirm", "Are you sure to delete this record?");
            if(!response)
            {
                return;
            }
            var categoriesViewModel = CategoriesViewModel.GetIntance();
            categoriesViewModel.DeleteCategory(this);
        }

        async void SelectedCategory()
        {
            var mainViewModel = MainViewModel.GetIntance();
            mainViewModel.Products = new ProductsViewModel(Productos);
            await navigationService.NavigateTo("ProductsView");
        }
    }
}
