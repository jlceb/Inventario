using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class EditCategoryViewModel : ViewModelBase
    {
        Category categorytoedit;

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

        public EditCategoryViewModel(Category category)
        {
            messageService = new MessageService();
            navigationService = new NavigationService();
            apiService = new ApiService();
            this.SaveCommand = new Command(this.Save);
            this.categorytoedit = category;
            Description = category.Description;
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

            categorytoedit.Description = Description;

            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.Put("http://productszuluapi.azurewebsites.net", "/api", "/Categories",
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken, categorytoedit);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            var categoriesViewModel = CategoriesViewModel.GetIntance();
            categoriesViewModel.UpdateCategory(categorytoedit);
            await navigationService.NavigateToBackOnMaster();

            IsEnabled = true;
            IsRunning = false;
        }
    }
}
