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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) // retrieve the elements from the table posts
            {
                var postTable = conn.Table<Post>().ToList(); // The ToList() method is LINQ

                // with linq, retrieve a list of posts ordered by category selecting only categories that are different (do not repeat them. That is what Distinct() does)
                var categories = (from p in postTable
                                  orderby p.CategoryId
                                  select p.CategoryName).Distinct().ToList();


                // Store in a dictionary called categoriesCountDictionary the count of posts in each category
                Dictionary<string, int> categoriesCountDictionary = new Dictionary<string, int>();
                int noCategoryAsignedToThisElement = 0;

                foreach (var category in categories)
                {
                    // With LINQ, retrieve the count of posts, where the category is a given one (defined by the loop)
                    var count = (from post in postTable
                                 where post.CategoryName == category
                                 select post).ToList().Count();

                    try
                    {
                        categoriesCountDictionary.Add(category, count);
                    }
                    catch (NullReferenceException nre) { noCategoryAsignedToThisElement++; }
                    catch (Exception ex) { }
                }

                categoriesCountDictionary.Add("No classified location", noCategoryAsignedToThisElement);

                categoriesListView.ItemsSource = categoriesCountDictionary;

                postCountLabel.Text = postTable.Count.ToString();
            }
        }
    }
}