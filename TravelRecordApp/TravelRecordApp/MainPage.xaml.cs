using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TravelRecordApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // set the image source:
            // VERY IMPORTANT! Right on the image -> Properties -> Build Action -> set as "Embedded Resources"
            var assembly = typeof(MainPage);  // source assembly where xmaarin has to look for the image
            iconImage.Source = ImageSource.FromResource("TravelRecordApp.Assets.Images.plane.png", assembly);
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Alert", "Introduce your email and password, please", "cancel");
                return;
            }
                
            await Navigation.PushAsync(new HomePage());

        }

        private async void cancelSubscriptionButton_Clicked(object sender, EventArgs e)
        {
            // Calling an async method (use async and await the result defining it as var)
            var result = await cancelSubscription();
            //resultCancelSusbscriptionLabel.Text = result;
        }

        // Definition and implementation of an async method (use async Task<returnTpye>)
        private async Task<string> cancelSubscription()
        {
            Thread.Sleep(2000);
            return "Cancelation succeded";
        }

        private void registerUserButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}
