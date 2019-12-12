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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void registerButton_Clicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text == confirmpasswordEntry.Text)
            {
                // We can register the user
            }
            else
            {
                DisplayAlert("Error", "Passwords don't match", "OK");
            }
        }
    }
}