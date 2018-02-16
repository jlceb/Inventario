using System;
using System.Collections.Generic;
using System.Text;

namespace MyStock.ViewModels
{
    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }

        public MainViewModel()
        {
            Login = new LoginViewModel();
        }
    }
}
