using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                this.Notify("Email");
            }
        }

        string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.Notify("Password");
            }
        }

        bool istoggled;
        public bool IsToggled
        {
            get
            {
                return istoggled;
            }
            set
            {
                istoggled = value;
                this.Notify("IsToggled");
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

        MessageService messageService;
        NavigationService navigationService;
        ApiService apiService;

        public LoginViewModel()
        {
            messageService = new MessageService();
            navigationService = new NavigationService();
            apiService = new ApiService();
            this.LoginCommand = new Command(this.Login);
            this.IsEnabled = true;
            this.IsToggled = true;
        }

        public ICommand LoginCommand
        {
            get;
            set;
        }

        public ICommand LoginWithFacebookCommand
        {
            get;
            set;
        }

        public ICommand RegisterNewUserCommand
        {
            get;
            set;
        }

        async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await messageService.SendMessage("Error", "You must enter an email");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await messageService.SendMessage("Error", "You must enter an password");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.GetToken("http://productszuluapi.azurewebsites.net", Email, Password);

            if (response == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", "The service is not available, please try later.");
                this.Password = null;
                return;
            }

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", response.ErrorDescription);
                this.Password = null;
                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;
            this.Password = null;

            var mainViewModel = MainViewModel.GetIntance();
            mainViewModel.tokenResponse = response;
            mainViewModel.Categories = new CategoriesViewModel();
            await navigationService.NavigateTo("CategoriesView");

        }

       
    }
}
