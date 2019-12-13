using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRecordApp.Model
{
    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }

        [MaxLength(250)]
        public string Expierience { get; set; }

        public string VenueName { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Distance { get; set; }

        public string UserId { get; set; }

        // Implementing MVVM -> Logic where it belongs: in the model.
        // That is why the functionality of inserting a post in the DB
        // is changed from NewTravelPage.xaml.cs to the Model Class of the Post
        public static async void Insert(Post post)
        {
            await App.MobileService.GetTable<Post>().InsertAsync(post);
        }

        public static async Task<List<Post>> Read()
        {
            return await App.MobileService.GetTable<Post>().Where(p => p.UserId == App.user.Id).ToListAsync();
        }

        public static Dictionary<string, int> GetCategories(List<Post> posts)
        {
            var categories = posts.OrderBy(p => p.CategoryId).Select(p => p.CategoryName).Distinct().ToList();


            // Store in a dictionary called categoriesCountDictionary the count of posts in each category
            Dictionary<string, int> categoriesCountDictionary = new Dictionary<string, int>();
            int noCategoryAsignedToThisElement = 0;

            foreach (var category in categories)
            {
                // With LINQ, retrieve the count of posts, where the category is a given one (defined by the loop)
                // 1st LINQ-Syntax:
                /*
                var count = (from post in postTable
                             where post.CategoryName == category
                             select post).ToList().Count();
                */

                // 2nd LINQ-Syntax:
                var count = posts.Where(p => p.CategoryName == category).ToList().Count();

                try
                {
                    categoriesCountDictionary.Add(category, count);
                }
                catch (NullReferenceException nre) { noCategoryAsignedToThisElement++; }
                catch (Exception ex) { }
            }

            categoriesCountDictionary.Add("No classified location", noCategoryAsignedToThisElement);

            return categoriesCountDictionary;
        }
    }
}
