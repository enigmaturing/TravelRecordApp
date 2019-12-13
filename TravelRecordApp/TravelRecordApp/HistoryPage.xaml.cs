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
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // SHOW POSTS STORED IN LOCAL-DB
            /*
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();

                // First step to implement databinding:
                // Establish the source of the binding (in this case, we do this from the .cs)
                // Here the source is the list of posts retrieved from the DB:
                postListView.ItemsSource = posts;
            };
            */

            // SHOW POSTS STORED IN AZURE-DB
            var posts = await Post.Read();  // MVVM -> Implementation of read posts is no in Model of Post -> var posts = await App.MobileService.GetTable<Post>().Where(p => p.UserId == App.user.Id).ToListAsync();
            postListView.ItemsSource = posts;
        }

        private async void PostListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPost = postListView.SelectedItem as Post;
            if (selectedPost != null)
            {
                await Navigation.PushAsync(new PostDetailPage(selectedPost));
            }
        }
    }
}