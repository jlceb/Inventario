using MyStock.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyStock.Services
{
    public class NavigationService
    {

        public void SetMainPage(string page)
        {
            switch (page)
            {
                case "LoginView":
                    Application.Current.MainPage = new NavigationPage(new LoginView());
                    break;
                case "MasterView":
                    Application.Current.MainPage = new MasterView();
                    break;
            }
        }

        public async Task NavigateOnMaster(string page)
        {
            App.Master.IsPresented = false;
            switch (page)
            {
                case "CategoriesView":
                    await App.Navigator.PushAsync(new CategoriesView());
                    break;
                case "ProductsView":
                    await App.Navigator.PushAsync(new ProductsView());
                    break;
                case "NewCategoryView":
                    await App.Navigator.PushAsync(new NewCategoryView());
                    break;
                case "EditCategoryView":
                    await App.Navigator.PushAsync(new EditCategoryView());
                    break;
                case "NewProductView":
                    await App.Navigator.PushAsync(new NewProductView());
                    break;
                case "EditProductView":
                    await App.Navigator.PushAsync(new EditProductView());
                    break;
                case "UbicationsView":
                    await App.Navigator.PushAsync(new UbicationsView());
                    break;
            }
        }

        public async Task NavigateOnLogin(string page)
        {
            switch (page)
            {
                case "NewCustomerView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new NewCustomerView());
                    break;
                case "LoginFacebookView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new LoginFacebookView());
                    break;
            }
        }

        public async Task NavigateToBackOnMaster()
        {
            await App.Navigator.PopAsync();
        }

        public async Task NavigateToBackOnLogin()
        {
            await MyStock.App.Current.MainPage.Navigation.PopAsync();
        }
             
    }
}
