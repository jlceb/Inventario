using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class NewCustomerViewModel : ViewModelBase
    {
        string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                this.Notify("FirstName");
            }
        }

        string lastName;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                this.Notify("LastName");
            }
        }

        string phone;
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                this.Notify("Phone");
            }
        }

        string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                this.Notify("Address");
            }
        }

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

        string confirm;
        public string Confirm
        {
            get
            {
                return confirm;
            }
            set
            {
                lastName = value;
                this.Notify("Confirm");
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

        public NewCustomerViewModel()
        {
            messageService = new MessageService();
            navigationService = new NavigationService();
            apiService = new ApiService();
            this.SaveCommand = new Command(this.Save);
            this.IsEnabled = true;
        }

        public ICommand SaveCommand
        {
            get;
            set;
        }

        async void Save()
        {

                if (string.IsNullOrEmpty(FirstName))
                {
                    await messageService.SendMessage("Error","You must enter a first name.");
                    return;
                }

                if (string.IsNullOrEmpty(LastName))
                {
                    await messageService.SendMessage("Error","You must enter a last name.");
                    return;
                }

                if (string.IsNullOrEmpty(Email))
                {
                    await messageService.SendMessage("Error","You must enter a email.");
                    return;
                }

                if (!EmailValidationService.IsValidEmail(Email))
                {
                    await messageService.SendMessage("Error","You must enter a valid email.");
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    await messageService.SendMessage("Error","You must enter a password.");
                    return;
                }

                if (Password.Length < 6)
                {
                    await messageService.SendMessage("Error","The password must have at least 6 characters length.");
                    return;
                }

                if (string.IsNullOrEmpty(Confirm))
                {
                    await messageService.SendMessage("Error","You must enter a password confirm.");
                    return;
                }

                if (!Password.Equals(Confirm))
                {
                    await messageService.SendMessage("Error","The password and confirm, does not match.");
                    return;
                }

                IsRunning = true;
                IsEnabled = false;

                var connection = await apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await messageService.SendMessage("Error", connection.Message);
                    return;
                }

                var customer = new Customer
                {
                    Address = Address,
                    CustomerType = 1,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    Password = Password,
                    Phone = Phone,
                };

                var response = await apiService.Post("http://productszuluapi.azurewebsites.net",
                    "/api","/Customers",customer);

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await messageService.SendMessage("Error",response.Message);
                    return;
                }

                var response2 = await apiService.GetToken("http://productszuluapi.azurewebsites.net",Email,Password);

                if (response2 == null)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await messageService.SendMessage("Error","The service is not available, please try latter.");
                    Password = null;
                    return;
                }

                if (string.IsNullOrEmpty(response2.AccessToken))
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await messageService.SendMessage("Error",response2.ErrorDescription);
                    Password = null;
                    return;
                }

                var mainViewModel = MainViewModel.GetIntance();
                mainViewModel.Categories = new CategoriesViewModel();
                await navigationService.NavigateToBackOnMaster();
                await navigationService.NavigateOnMaster("CategoriesView");
                IsRunning = false;
                IsEnabled = true;

        }
    }
}
