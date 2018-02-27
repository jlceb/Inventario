using MyStock.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyStock.Services
{
    public class NavigationService
    {
        public async Task NavigateTo(string page)
        {
            switch (page)
            {
                case "CategoriesView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new CategoriesView());
                    break;
                case "ProductsView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new ProductsView());
                    break;
                case "NewCategoryView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new NewCategoryView());
                    break;
                case "EditCategoryView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new EditCategoryView());
                    break;
                default:
                    break;
            }
        }

        public async Task NavigateToBack()
        {
            await MyStock.App.Current.MainPage.Navigation.PopAsync();
        }
             
    }
}
