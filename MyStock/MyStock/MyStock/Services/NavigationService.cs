using MyStock.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyStock.Services
{
    public class NavigationService
    {
        public async void NavigateTo(string page)
        {
            switch (page)
            {
                case "CategoriesView":
                    await MyStock.App.Current.MainPage.Navigation.PushAsync(new CategoriesView());
                    break;
                default:
                    break;
            }
        }
    }
}
