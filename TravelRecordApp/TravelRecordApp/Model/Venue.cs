using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Helpers;

namespace TravelRecordApp.Model
{
    public class Location
    {
        public string address { get; set; }
        public string crossStreet { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int distance { get; set; }
        public string cc { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public IList<string> formattedAddress { get; set; }
        public string postalCode { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public string shortName { get; set; }
        public bool primary { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public IList<Category> categories { get; set; }

        public async static Task<List<Venue>> GetVenues(double latitude, double longitude)
        {
            List<Venue> venues = new List<Venue>();

            var url = VenueRoot.GenerateURL(latitude, longitude);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);  // Get response to the http request
                var json = await response.Content.ReadAsStringAsync(); // Get the json object contained in the content of the response
                var venueRoot = JsonConvert.DeserializeObject<VenueRoot>(json);  // Deserialize the json object to an object VenueRoot

                venues = venueRoot.response.venues as List<Venue>;  // Get the list of venues stored in the field response.venues of the class venueRoot
            }

            return venues;
        }
    }

    public class Response
    {
        public IList<Venue> venues { get; set; }
    }

    public class VenueRoot
    {
        public Response response { get; set; }

        public static string GenerateURL(double latitue, double longitude)
        {
            return string.Format(Constants.VENUE_SEARCH, latitue.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture), Constants.CLIENT_ID, Constants.CLIENT_SECTRET, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
