using MyStock.Services;
using MyStock.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.Models
{
    public class Menu
    {
        public string Icon { get; set; }
        public string Option { get; set; }
        public string PageName { get; set; }

        NavigationService navigationService;

        public Menu()
        {
            navigationService = new NavigationService();
            this.NavigateCommand = new Command(this.Navigate);
        }


        public ICommand NavigateCommand
        {
            get;
            set;
        }

        async void Navigate()
        {
            switch(PageName)
            {
                case "LoginView":
                    {
                        MainViewModel.GetIntance().Login = new LoginViewModel();
                        navigationService.SetMainPage("LoginView");
                        break;
                    }
                case "UbicationsView":
                    {
                        MainViewModel.GetIntance().Ubications = new UbicationsViewModel();
                        await navigationService.NavigateOnMaster("UbicationsView");
                        break;
                    }
            }
        }

    }
}
