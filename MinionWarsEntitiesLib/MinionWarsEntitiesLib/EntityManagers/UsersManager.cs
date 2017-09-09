using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using System.Data.Entity.Spatial;
using MinionWarsEntitiesLib.Structures;

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
                        newMovement.activity_saturation = 1;
                        newMovement.event_saturation = 0;
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

        public static void SetSubscription(int id, int sub)
        {
            using (var db = new MinionWarsEntities())
            {
                Users u = db.Users.Find(id);
                if (u.subscription == null) u.subscription = 0;
                u.subscription += sub;

                db.Users.Attach(u);
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
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
                //List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => (x.occurence - DateTime.Now).TotalMinutes <= time).ToList();
                List<UserMovementHistory> umh = db.UserMovementHistory.ToList();
                return umh;
            }
        }

        public static List<UserMovementHistory> GetHighestEventSaturationLocations(decimal percentage)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserMovementHistory> umh = db.UserMovementHistory.OrderByDescending(x => x.event_saturation).ToList();
                int toTake = Convert.ToInt32(Math.Floor(percentage * umh.Count));
                if (toTake == 0) toTake = 1;
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

        public static void UpdateActivitySaturations()
        {
            DateTime now = DateTime.Now;
            using (var db = new MinionWarsEntities())
            {
                List < UserMovementHistory > history = db.UserMovementHistory.Where(x => x.activity_saturation > 0).ToList();
                foreach (UserMovementHistory h in history)
                {
                    var difference = now - h.occurence;
                    var value = Math.Floor((difference.TotalSeconds - 300 * difference.TotalMinutes) / 5);
                    h.activity_saturation -= 0.01*value;
                    if (h.activity_saturation <= 0)
                    {
                        db.UserMovementHistory.Remove(h);
                    }
                    else
                    {
                        db.UserMovementHistory.Attach(h);
                        db.Entry(h).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                //check last activity;
                history = history.OrderByDescending(x => x.occurence).ToList();
                if ((now - history.First().occurence).TotalMinutes >= 15)
                {
                    Users user = db.Users.Find(history.First().users_id);
                    user.online = 0;

                    if ((now - history.First().occurence).TotalHours >= 24)
                    {
                        user.online = -1;
                    }

                    db.Users.Attach(user);
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                }

                //check for discovery
                CampManager.CheckForDiscovery(history.First().location, 5000);

                db.SaveChanges();
            }
        }
    }
}
