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
        public static void UpdateUserPosition(int user_id, double longitude, double latitude)
        {
            using (var db = new MinionWarsEntities())
            {
                Users user = null;
                user = db.Users.Find(user_id);

                if (user != null)
                {
                    if (user.location != null)
                    {
                        UserMovementHistory newMovement = new UserMovementHistory();
                        newMovement.users_id = user_id;
                        newMovement.occurence = DateTime.Now;
                        newMovement.location = user.location;
                        db.UserMovementHistory.Add(newMovement);
                    }

                    var point = string.Format("POINT({1} {0})", latitude, longitude);
                    user.location = DbGeography.FromText(point);
                    db.Users.Attach(user);
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
            }
        }

        public static Users GetUserData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.Users.Find(id);
            }
        }

        public static Battlegroup GetPersonalBattlegroup(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.Battlegroup.Find(id);
            }
        }

        public static List<UserMovementHistory> GetLatestLocations(int time)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => (x.occurence - DateTime.Now).TotalMinutes <= 15).ToList();
                return umh;
            }
        }

        public static List<UserMovementHistory> GetHighestEventSaturationLocations(double percentage)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserMovementHistory> umh = db.UserMovementHistory.OrderByDescending(x => x.event_saturation).ToList();
                int toTake = Convert.ToInt32(percentage * umh.Count);
                List<UserMovementHistory> limitedUmh = umh.Take(toTake).ToList();

                return limitedUmh;
            }
        }

        public static void UpdateEventSaturations(DbGeography loc, double coef)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => x.location.Distance(loc).Value <= 500).ToList();
                foreach (UserMovementHistory u in umh)
                {
                    if (u.event_saturation == null) u.event_saturation = 0;
                    u.event_saturation += coef * (100 - u.location.Distance(loc) / 5);
                    if (u.event_saturation < 0) u.event_saturation = 0;
                    db.UserMovementHistory.Attach(u);
                    db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}
