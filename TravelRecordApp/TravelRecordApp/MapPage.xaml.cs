using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private bool hasLocationPermission = false;

        public MapPage()
        {
            InitializeComponent();

            // Check actual permission status and if not granted, ask the user for it
            GetPermissions();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (hasLocationPermission)
            {
                // Si tenemos persmisos de posición, creamos un locator de la posición actual,
                // creamos un event handler que se llame cuando la posición cambia
                // y comenzamos a escuchar cambios en la posición sin intervalo de tiempo, cuando
                // el cambio sea mayor a 100 metros
                var locator = CrossGeolocator.Current;
                locator.PositionChanged += Locator_PositionChanged;
                await locator.StartListeningAsync(TimeSpan.Zero, 100);
            }

            GetLocation();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();

                DisplayPinsInMap(posts);
            };
        }

        private void DisplayPinsInMap(List<Post> posts)
        {
            foreach (var post in posts)
            {
                try
                {
                    var position = new Xamarin.Forms.Maps.Position(post.Latitude, post.Longitude);
                    var pin = new Xamarin.Forms.Maps.Pin()
                    {
                        Type = PinType.SavedPin,
                        Position = position,
                        Label = post.VenueName,
                        Address = post.Address
                    };
                    locationMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre){}
                catch (Exception) {}
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Cuando salimos del mapa, parar de escuchar la posición
            // y eliminamos la subscripción al evento que se llama cuando cambia la posición:
            // THESE NEXT TWO LINES ARE VERY IMPORTANT TO PREVENT A RUNTIME ERROR otherwise
            // we would go out of the mapview withouth stoping listening to the position of
            // the gps (which does not cause any error), but when we come back an the onAppearin()
            // method is run again, the current position will be started to be listened again
            // and THAT would cause a runtime error, if we are already listening to it
            CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= Locator_PositionChanged;
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        // El método MoveMap() centra el mapa en la posción que le pasemos: puede ser la del usuario actualmente
        // ó una posición general (madrid), en caso de que el usuario no haya dado permisos.
        private void MoveMap(Plugin.Geolocator.Abstractions.Position position)
        {
            var mapCenter = new Position(position.Latitude, position.Longitude);
            var mapSpan = new MapSpan(mapCenter, 0.01, 0.01);
            locationMap.MoveToRegion(mapSpan);
        }

        private async void GetLocation()
        {
            if (hasLocationPermission)
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();  // Posición actual del usuario

                MoveMap(position);
            }
            else
            {
                var mapCenter = new Position(40.4165, -3.70256);  // Madrid
                var mapSpan = new MapSpan(mapCenter, 15, 15);

                locationMap.MoveToRegion(mapSpan);
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
                    hasLocationPermission = true;

                    GetLocation();
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
