using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class Geolocations
    {
        public static double GetDistance(Location loc1, Location loc2)
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
        }

        public static Location PerformMovement(Location current, Location destination, int speed)
        {
            Location newCurrent = new Location();

            if(Math.Abs(destination.latitude - current.latitude) < 2)
            {
                newCurrent.latitude = current.latitude;
            }
            else
            {
                if (destination.latitude > current.latitude)
                {
                    newCurrent.latitude = current.latitude + speed / (1852 * 60);
                }
                else {
                    newCurrent.latitude = current.latitude - speed / (1852 * 60);
                }
            }

            if (Math.Abs(destination.longitude - current.longitude) < 2)
            {
                newCurrent.longitude = current.longitude;
            }
            else
            {
                if (destination.longitude > current.longitude)
                {
                    newCurrent.longitude = current.longitude + speed / (1852 * 60);
                }
                else {
                    newCurrent.longitude = current.longitude - speed / (1852 * 60);
                }
            }

            return newCurrent;
        }
    }
}
