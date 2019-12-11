using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;

namespace TravelRecordApp.Logic
{
    public class VenueLogic
    {
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
}
