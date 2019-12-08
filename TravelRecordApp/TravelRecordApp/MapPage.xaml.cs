using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            // Check actual permission status and if not granted, ask the user for it
            GetPermissions();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Show or hide button "locate user" depending on the actual status of permission
            // This has to be done each time the MapPage appears,to refresh the visibility of
            // the user, just in case he/she has changed its mind since last choice on the app
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            if (status == PermissionStatus.Granted)
            {
                locationMap.IsShowingUser = true;
            }
            else
            {
                locationMap.IsShowingUser = false;
            }
            
        }

        private async void GetPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await DisplayAlert("Location needed", "We need access to your location to use this feature", "I am cool with that!");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                    if (results.ContainsKey(Permission.LocationWhenInUse))
                    {
                        status = results[Permission.LocationWhenInUse];
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    locationMap.IsShowingUser = true;
                }
                else
                {
                    await DisplayAlert("Location access denied", "You didn't give us permission to access yout location, so we can't show you on the map.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
            
        }
    }
}
