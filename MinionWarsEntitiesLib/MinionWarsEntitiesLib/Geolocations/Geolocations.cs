using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class Geolocations
    {
        public static DbGeography PerformMovement(DbGeography current, DateTime lastMovement, Orders orders, int speed)
        {
            DbGeography newLoc = null;
            double newLat = 0;
            double newLon = 0;

            var diffInSeconds = (DateTime.Now - lastMovement).TotalSeconds;
            var distance = (double)speed * diffInSeconds;

            var axisDistance = Math.Sqrt(Math.Pow(distance, 2) / 2);
            var totalAxisDistance = Math.Sqrt(Math.Pow(current.Distance(orders.location).Value, 2) / 2);

            if (totalAxisDistance < axisDistance) axisDistance = totalAxisDistance;

            double toMove = (double)((decimal)axisDistance / (1852m * 60m));
            double checkDiff = (double)(2m / (1852m * 60m));

            if (Math.Abs(orders.location.Latitude.Value - current.Latitude.Value) < checkDiff)
            {
                newLat = current.Latitude.Value;
            }
            else
            {
                if (orders.location.Latitude.Value > current.Latitude.Value)
                {
                    newLat = current.Latitude.Value + toMove;
                }
                else {
                    newLat = current.Latitude.Value - toMove;
                }
            }

            if (Math.Abs(orders.location.Longitude.Value - current.Longitude.Value) < checkDiff)
            {
                newLon = current.Longitude.Value;
            }
            else
            {
                if (orders.location.Longitude.Value > current.Longitude.Value)
                {
                    newLon = current.Longitude.Value + toMove;
                }
                else {
                    newLon = current.Longitude.Value - toMove;
                }
            }

            var point = string.Format("POINT({1} {0})", newLat, newLon);
            newLoc = DbGeography.FromText(point);

            return newLoc;
        }

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

        public static string GetNewDirections(DbGeography current, Orders orders)
        {
            return OrdersParser.ParseDirections(GetDirections(orders.location.Latitude.Value, orders.location.Longitude.Value, current.Latitude.Value, current.Longitude.Value).Result);
        }

        public static string GetCaravanDirections(DbGeography current, DbGeography destination)
        {
            return OrdersParser.ParseDirections(GetDirections(destination.Latitude.Value, destination.Longitude.Value, current.Latitude.Value, current.Longitude.Value).Result);
        }

        public static DbGeography GetDirectionMovement(DbGeography current, Orders orders)
        {
            if (orders.directions == null) orders.directions = OrdersParser.ParseDirections(GetDirections(orders.location.Latitude.Value, orders.location.Longitude.Value, current.Latitude.Value, current.Longitude.Value).Result);

            string directions = OrdersParser.UpdateNextLoc(orders.directions);

            using (var db = new MinionWarsEntities())
            {
                orders.directions = directions;

                db.Orders.Attach(orders);
                db.Entry(orders).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return OrdersParser.GetNextLoc(directions);
        }

        public static DbGeography GetCaravanDirectionMovement(Caravan car, DbGeography destination)
        {
            if (car.directions == null) car.directions = OrdersParser.ParseDirections(GetDirections(destination.Latitude.Value, destination.Longitude.Value, car.location.Latitude.Value, car.location.Longitude.Value).Result);

            string directions = OrdersParser.UpdateNextLoc(car.directions);

            using (var db = new MinionWarsEntities())
            {
                car.directions = directions;

                db.Caravan.Attach(car);
                db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return OrdersParser.GetNextLoc(directions);
        }

        public static DbGeography PerformDirectionMovement(DbGeography current, DbGeography next, DateTime lastMovement, int speed)
        {
            DbGeography newLoc = null;
            double newLat = 0;
            double newLon = 0;

            var diffInSeconds = (DateTime.Now - lastMovement).TotalSeconds;
            var distance = (double)speed * diffInSeconds;

            if (distance > current.Distance(next))
            {
                newLoc = next;
            }
            else
            {
                var axisDistance = Math.Sqrt(Math.Pow(distance, 2) / 2);
                var totalAxisDistance = Math.Sqrt(Math.Pow(current.Distance(next).Value, 2) / 2);

                if (totalAxisDistance < axisDistance) axisDistance = totalAxisDistance;

                double toMove = (double)((decimal)axisDistance / (1852m * 60m));
                double checkDiff = (double)(2m / (1852m * 60m));

                if (Math.Abs(next.Latitude.Value - current.Latitude.Value) < checkDiff)
                {
                    newLat = current.Latitude.Value;
                }
                else
                {
                    if (next.Latitude.Value > current.Latitude.Value)
                    {
                        newLat = current.Latitude.Value + toMove;
                    }
                    else {
                        newLat = current.Latitude.Value - toMove;
                    }
                }

                if (Math.Abs(next.Longitude.Value - current.Longitude.Value) < checkDiff)
                {
                    newLon = current.Longitude.Value;
                }
                else
                {
                    if (next.Longitude.Value > current.Longitude.Value)
                    {
                        newLon = current.Longitude.Value + toMove;
                    }
                    else {
                        newLon = current.Longitude.Value - toMove;
                    }
                }

                var point = string.Format("POINT({1} {0})", newLat, newLon);
                newLoc = DbGeography.FromText(point);
            }

            return newLoc;
        }

        public static Task<string> GetDirections(double lat, double lon, double clat, double clon)
        {
            string apiCall = "https://maps.googleapis.com/maps/api/directions/json?";
            string origin = "&origin=" + clat.ToString() + "," + clon.ToString();
            string destination = "&destination=" + lat.ToString() + "," + lon.ToString();
            string mode = "&mode=walking";
            string units = "&units=metric";
            string apiKey = "&key=AIzaSyCS8CA5fO7JvUk_s4hV7tMsDJkeY5cvhIo";

            string call = origin + destination + mode + units + apiKey;

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

        public static List<Camp> InitiateDiscovery(DbGeography loc)
        {
            string places = GetPlaces(loc.Latitude.Value, loc.Longitude.Value, 1000, "restaurant").Result;
            List<Camp> newCamps = new List<Camp>();
            List<DbGeography> newLocs = new List<DbGeography>();
            //parse, create new camp
            dynamic obj = JsonConvert.DeserializeObject(places);
            //dynamic obj = JObject.Parse(places);
            if(obj["results"] != null)
            {
                for (int i = 0; i < obj["results"].Count; i++)
                {
                    dynamic place = obj["results"][i];
                    var point = string.Format("POINT({1} {0})", place["geometry"]["location"]["lat"], place["geometry"]["location"]["lng"]);
                    DbGeography newLoc = DbGeography.FromText(point);
                    newLocs.Add(newLoc);
                }
            }

            using (var db = new MinionWarsEntities())
            {
                foreach (DbGeography l in newLocs)
                {
                    //Console.WriteLine("Discovery found");
                    List<Camp> check = new List<Camp>();
                    check = db.Camp.Where(x => x.location.Distance(l) <= 250).ToList();
                    if (check.Count == 0)
                    {
                        Camp nc = CampManager.CreateCamp(-1, l, "Neutral Camp");
                        newCamps.Add(nc);
                    }

                }
            }

            return newCamps;
        }
    }
}
