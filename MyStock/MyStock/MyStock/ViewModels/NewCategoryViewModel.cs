using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class NewCategoryViewModel : ViewModelBase
    {
        string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                this.Notify("Description");
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

        public NewCategoryViewModel()
        {
            messageService = new MessageService();
            navigationService = new NavigationService();
            apiService = new ApiService();
            this.SaveCommand = new Command(this.Save);

            IsEnabled = true;
        }

        public ICommand SaveCommand
        {
            get;
            set;
        }

        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await messageService.SendMessage("Error", "You must enter a description category.");
            }

            IsEnabled = false;
            IsRunning = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", connection.Message);
                return;
            }

            var category = new Category
            {
                Description = this.Description,
            };

            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.Post("http://productszuluapi.azurewebsites.net", "/api","/Categories",
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken,category);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            category = (Category)response.Result;
            var categoriesViewModel = CategoriesViewModel.GetIntance();
            categoriesViewModel.AddCategory(category);
            await navigationService.NavigateToBack();

            IsEnabled = true;
            IsRunning = false;
        }
    }
}
