using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
        public string referralId { get; set; }
        public bool hasPerk { get; set; }
    }

    public class VenueRoot
    {
        public static string GenerateURL(double latitue, double longitude)
        {
            return string.Format(Constants.VENUE_SEARCH, latitue.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture), Constants.CLIENT_ID, Constants.CLIENT_SECTRET, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
