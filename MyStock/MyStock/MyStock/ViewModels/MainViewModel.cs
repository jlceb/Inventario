using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyStock.ViewModels
{
    public class MainViewModel
    {
        public TokenResponse tokenResponse { get; set; }
        public LoginViewModel Login { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public ProductsViewModel Products { get; set; }
        public NewCategoryViewModel NewCategory { get; set; }
        public EditCategoryViewModel EditCategory { get; set; }
        public NewProductViewModel NewProduct { get; set; }
        public Category Category { get; set; }

        NavigationService navigationService;

        public ICommand NewCategoryCommand
        {
            get;
            set;
        }

        public async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.NavigateTo("NewCategoryView");
        }

        public MainViewModel()
        {
            navigationService = new NavigationService();
            Login = new LoginViewModel();
        }

        //Singlenton

        static MainViewModel instance;

        public static MainViewModel GetIntance()
        {
            if (instance == null)
                return new MainViewModel();
            else
                return instance;
        }
    }
}
