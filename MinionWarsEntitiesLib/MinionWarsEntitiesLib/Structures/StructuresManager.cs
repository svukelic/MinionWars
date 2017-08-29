using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Structures
{
    public static class StructuresManager
    {
        public static Task<string> GetPlaces(double lat, double lon, int radius, string type)
        {
            string apiCall = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
            string location = "&location=" + lat.ToString() + "," + lon.ToString();
            string radiusCall = "&radius=" + radius.ToString();
            string typeCall = "&type=" + type;
            string apiKey = "&key=AIzaSyCS8CA5fO7JvUk_s4hV7tMsDJkeY5cvhIo";

            string call = location + radiusCall + typeCall + apiKey;

            return CallApi(apiCall + call);
        }

        private static async Task<string> CallApi(string call)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(call))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                if (result != null)
                {
                    return result;
                }
                else return null;
            }
        }
    }
}
