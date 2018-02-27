using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStock.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        List<Category> listCategories;

        bool isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                this.Notify("IsRefreshing");
            }
        }

        ObservableCollection<Category> _categories;
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
            instance = this;
            apiService = new ApiService();
            messageService = new MessageService();
            this.RefreshCommand = new Command(this.LoadCategories);
            LoadCategories();
        }

        public ICommand RefreshCommand
        {
            get;
            set;
        }

        async void LoadCategories()
        {
            var connection = await apiService.CheckConnection();
            IsRefreshing = true;

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

            listCategories = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(listCategories.OrderBy(x => x.Description));
            IsRefreshing = false;
        }

        public void AddCategory(Category newCategory)
        {
            IsRefreshing = true;
            listCategories.Add(newCategory);
            Categories = new ObservableCollection<Category>(listCategories.OrderBy(x => x.Description));
            IsRefreshing = false;
        }

        public void UpdateCategory(Category newCategory)
        {
            IsRefreshing = true;
            var oldCategory = listCategories.Where(c => c.CategoryId == newCategory.CategoryId).FirstOrDefault();
            oldCategory = newCategory;
            Categories = new ObservableCollection<Category>(listCategories.OrderBy(x => x.Description));
            IsRefreshing = false;
        }

        public async void DeleteCategory(Category categorytodelete)
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await messageService.SendMessage("Error", connection.Message);
                return;
            }


            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.Delete("http://productszuluapi.azurewebsites.net", "/api", "/Categories",
                mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken, categorytodelete);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await messageService.SendMessage("Error", response.Message);
                return;
            }

            listCategories.Remove(categorytodelete);

            Categories = new ObservableCollection<Category>(listCategories.OrderBy(x => x.Description));
            IsRefreshing = false;
        }


        //Singlenton

        static CategoriesViewModel instance;

        public static CategoriesViewModel GetIntance()
        {
            if (instance == null)
                return new CategoriesViewModel();
            else
                return instance;
        }
    }
}
