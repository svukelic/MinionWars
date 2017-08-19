using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class Geolocations
    {
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

        public static DbGeography PerformMovement(DbGeography current, DbGeography destination, int speed)
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
        }
    }
}
