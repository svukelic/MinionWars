using MinionWarsEntitiesLib.Models;
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

            /*Console.WriteLine("OLD LOC: " + current.ToString());
            Console.WriteLine("NEW LOC: " + newLoc.ToString());*/

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

        public static void CheckForDiscovery()
        {

        }

        /*public static double GetDistance(Location loc1, Location loc2)
        {
            double rlat1 = Math.PI * loc1.latitude / 180;
            double rlat2 = Math.PI * loc2.latitude / 180;
            double theta = loc1.longitude - loc2.longitude;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            dist = dist * 1000;

            return dist;
        }*/

        /*public static DbGeography PerformMovement(DbGeography current, DbGeography destination, int speed)
        {
            DbGeography newCurrent = null;
            double newLat = 0;
            double newLon = 0;

            if (Math.Abs(destination.Latitude.Value - current.Latitude.Value) < 2)
            {
                newLat = current.Latitude.Value;
            }
            else
            {
                if (destination.Latitude.Value > current.Latitude.Value)
                {
                    newLat = current.Latitude.Value + speed / (1852 * 60);
                }
                else {
                    newLat = current.Latitude.Value - speed / (1852 * 60);
                }
            }

            if (Math.Abs(destination.Longitude.Value - current.Longitude.Value) < 2)
            {
                newLon = current.Longitude.Value;
            }
            else
            {
                if (destination.Longitude.Value > current.Longitude.Value)
                {
                    newLon = current.Longitude.Value + speed / (1852 * 60);
                }
                else {
                    newLon = current.Longitude.Value - speed / (1852 * 60);
                }
            }

            var point = string.Format("POINT({1} {0})", newLat, newLon);
            newCurrent = DbGeography.FromText(point);

            return newCurrent;
        }*/
    }
}
