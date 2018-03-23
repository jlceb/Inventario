using MyStock.Models;
using MyStock.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyStock
{
	public partial class App : Application
	{
        public static NavigationPage Navigator { get; internal set; }
        public static MasterView Master { get; internal set; }

        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new LoginView());

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public static Action LoginFacebookFail
        {
            get
            {
                return new Action(() => Current.MainPage = new NavigationPage(new LoginView()));
            }
        }

        public static void LoginFacebookSuccess(FacebookResponse profile)
        {
            Current.MainPage = new MasterView();
        }
    }
}
