using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyStock.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        public ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                this.Notify("Categories");
            }
        }

        ApiService apiService;
        MessageService messageService;

        public CategoriesViewModel()
        {
            apiService = new ApiService();
            messageService = new MessageService();
            LoadCategories();
        }

        async void LoadCategories()
        {
            var connection = await apiService.CheckConnection();

            if(!connection.IsSuccess)
            {
                await messageService.SendMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetIntance();
            var response = await apiService.GetList<Category>("http://productszuluapi.azurewebsites.net", "/api", "/Categories", 
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken);

            if (!response.IsSuccess)
            {
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            var listCategories = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(listCategories.OrderBy(x => x.Description)); 
        }
    }
}
