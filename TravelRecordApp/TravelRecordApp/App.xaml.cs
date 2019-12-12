using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;
        public static MobileServiceClient client = new MobileServiceClient("https://travelrecordappjgm.azurewebsites.net");

        public App()
        {
            InitializeComponent();

            // Adding new NavigationPage() so that we can navigate to other pages having
            // the back arrow on the top, by calling await Navigation.PushAsync() from the MainPage.xaml.cs
            MainPage = new NavigationPage(new MainPage());
        }

        // The next constructor states the locastion of the local database for our app
        public App(string databaseLocation)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());

            DatabaseLocation = databaseLocation;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
