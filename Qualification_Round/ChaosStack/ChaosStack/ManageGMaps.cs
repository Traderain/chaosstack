using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace ChaosStack
{
    class ManageGMaps
    {
        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static string GenerateMapsUrl(string latitude, string longitude, string destination)
        {
            var dateTime = ToUnixTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 7, 30, 0));

            return $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={latitude.Replace(',', '.')},{longitude.Replace(',', '.')}&destinations={destination}&mode=driving&arrival_time={dateTime}&key=AIzaSyCI0ypRi4huBml1WASi48w6qH2-Bp_CgDs";
        }

        public static JObject GetGMapsResult(string latitude, string longitude, string destination)
        {
            var uri = GenerateMapsUrl(latitude, longitude, destination);
            var gMapsRes = HTTP_Requests.Send_GetRequest(uri);
            return JObject.Parse(gMapsRes);
        }
    }
}
