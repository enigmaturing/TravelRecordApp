using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TravelRecordApp.Helpers;

namespace TravelRecordApp.Model
{
    public class Venue
    {
        public static string GenerateURL(double latitue, double longitude)
        {
            return string.Format(Constants.VENUE_SEARCH, latitue.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture), Constants.CLIENT_ID, Constants.CLIENT_SECTRET, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
