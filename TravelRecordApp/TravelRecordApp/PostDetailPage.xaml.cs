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
    public partial class PostDetailPage : ContentPage
    {
        Post selectedPost; 

        public PostDetailPage(Post selectedPost)
        {
            InitializeComponent();
            this.selectedPost = selectedPost;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            selectedPost.Expierience = expierienceEntry.Text;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Update(selectedPost);

                if (rows > 0)
                    DisplayAlert("Success", "Expiereince sucessfully updated", "Great!");
                else
                    DisplayAlert("Failure", "Expiereince failed to be updated", "OK");
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Delete post from Azure-DB
            try
            {
                var post = (await App.MobileService.GetTable<Post>().Where(p => p.Id == selectedPost.Id).ToListAsync()).FirstOrDefault<Post>();
                await App.MobileService.GetTable<Post>().DeleteAsync(post);
                await DisplayAlert("Success", "Expiereince sucessfully deleted", "Great!");
                await Navigation.PushAsync(new HistoryPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", "Expiereince failed to be deleted", "OK");
            }

            // Delete post from Local-DB
            //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
            //    conn.CreateTable<Post>();
            //    int rows = conn.Delete(selectedPost);

            //    if (rows > 0)
            //        DisplayAlert("Success", "Expiereince sucessfully deleted", "Great!");
            //    else
            //        DisplayAlert("Failure", "Expiereince failed to be deleted", "OK");
            //}
        }
    }
}