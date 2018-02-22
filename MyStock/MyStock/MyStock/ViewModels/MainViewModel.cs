using MyStock.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyStock.ViewModels
{
    public class MainViewModel
    {
        public TokenResponse tokenResponse { get; set; }
        public LoginViewModel Login { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public ProductsViewModel Products { get; set; }

        public MainViewModel()
        {
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
