using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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
        ApiService apiService;

        public LoginViewModel()
        {
            messageService = new MessageService();
            apiService = new ApiService();
            IsEnabled = true;
            IsToggled = true;
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

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (connection.IsSuccess)
            {

            }
        }

       
    }
}
