using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using System.Data.Entity.Spatial;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public static class UsersManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();

        public static void UpdateUserPosition(int user_id, float longitude, float latitude)
        {
            MinionWarsEntitiesLib.Models.Users user = null;
            user = db.Users.Find(user_id);
            
            if(user != null)
            {
                UserMovementHistory newMovement = new UserMovementHistory();
                newMovement.users_id = user_id;
                newMovement.occurence = DateTime.Now;
                var point = string.Format("POINT({1} {0})", latitude, longitude);
                newMovement.location = DbGeography.FromText(point);

                db.UserMovementHistory.Add(newMovement);
                db.SaveChanges();
            }
        }

        public static List<UserMovementHistory> GetLatestLocations(int time)
        {
            List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => (x.occurence - DateTime.Now).TotalMinutes <= 15).ToList();
            return umh;
        }

        public static List<UserMovementHistory> GetHighestEventSaturationLocations(double percentage)
        {
            List<UserMovementHistory> umh = db.UserMovementHistory.OrderByDescending(x => x.event_saturation).ToList();
            int toTake = Convert.ToInt32(percentage * umh.Count);
            List<UserMovementHistory> limitedUmh = umh.Take(toTake).ToList();

            return limitedUmh;
        }

        public static void UpdateEventSaturations(DbGeography loc, double coef)
        {
            List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => x.location.Distance(loc).Value <= 500).ToList();
            foreach(UserMovementHistory u in umh)
            {
                if (u.event_saturation == null) u.event_saturation = 0;
                u.event_saturation += coef * (100 - u.location.Distance(loc) / 5);
                if (u.event_saturation < 0) u.event_saturation = 0;
            }
        }

        public static string TestData()
        {
            //string test = db.UserMovementHistory.Last().occurence.ToString();
            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            List<UserMovementHistory> umh = db.UserMovementHistory.ToList();
            List<Users> test = db.Users.ToList();
            return umh.First().location.ToString();
        }
    }
}
