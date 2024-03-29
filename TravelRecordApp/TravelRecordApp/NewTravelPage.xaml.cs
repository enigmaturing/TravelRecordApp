﻿using Plugin.Geolocator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTravelPage : ContentPage
    {
        public NewTravelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Get the user's location and the venues on its surronunding thanks to the logic
            // implemented in our class VenueLogic
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            var venues = await Venue.GetVenues(position.Latitude, position.Longitude);
            venueListView.ItemsSource = venues;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedVenue = venueListView.SelectedItem as Venue;  // We can do the cast to Venue because the source of the veueListView is set to venues, which is a List of venues
                var firstCategory = selectedVenue.categories.FirstOrDefault();

                Post post = new Post()
                {
                    Expierience = expierenceEntry.Text,
                    CategoryId = firstCategory.id,
                    CategoryName = firstCategory.name,
                    Address = selectedVenue.location.address,
                    Latitude = selectedVenue.location.lat,
                    Longitude = selectedVenue.location.lng,
                    VenueName = selectedVenue.name,
                    Distance = selectedVenue.location.distance,
                    UserId = App.user.Id
                };

                // Insert the post into the local SQL DB
                //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                //{
                //    conn.CreateTable<Post>();
                //    int rows = conn.Insert(post);

                //    if (rows > 0)
                //        DisplayAlert("Success", "Expiereince sucessfully inserted", "Great!");
                //    else
                //        DisplayAlert("Failure", "Expiereince failed to be inserted", "OK");
                //}

                // Insert the post into the azure db
                Post.Insert(post);  // -> MVVM: The implementation is now in the Model of Post -> await App.MobileService.GetTable<Post>().InsertAsync(post);
                await DisplayAlert("Success", "Expiereince sucessfully inserted", "Great!");  // This line will only be executed AFTER the previous line is finished, because it is an async-method marked with await
                await Navigation.PopAsync();
            } 
            catch (NullReferenceException nre)
            {
                await DisplayAlert("Failure", "Expiereince failed to be inserted", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", "Expiereince failed to be inserted", "OK");
            }
        }
    }
}