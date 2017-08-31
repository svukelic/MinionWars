using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.AiManagers
{
    public static class OrdersManager
    {
        //static MinionWarsEntities db = new MinionWarsEntities();

        public static Orders GiveNewOrders(Battlegroup bg, string orders, DbGeography destination)
        {
            Orders o = new Orders();
            o.name = orders;
            o.desc_text = "test";
            //o.lastMovement = DateTime.Now;

            if (destination != null) o.location = destination;
            else
            {
                o.location = GiveRandomDestination(bg, 250);
                o.directions = Geolocations.Geolocations.GetNewDirections(bg.location, o);
            }

            using (var db = new MinionWarsEntities())
            {
                db.Orders.Add(o);
                db.SaveChanges();
            }

            //ContinueOrders(bg, o);

            return o;
        }

        public static Orders ContinueOrders(Battlegroup bg, Orders o)
        {
            bool death = false;
            using (var db = new MinionWarsEntities())
            {
                ModifierCoeficients ttl = db.ModifierCoeficients.Find(26);
                var difference = (DateTime.Now - bg.creation.Value).TotalMinutes;
                if (difference > ttl.value)
                {
                    bg.location = null;
                    bg.orders_id = null;
                    death = true;

                    db.Battlegroup.Attach(bg);
                    db.Entry(bg).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            if (!death)
            {
                DbGeography newLoc = o.location;
                switch (o.name)
                {
                    case "roam":
                        o.name = "wait";
                        break;
                    case "wait":
                        var waitTime = (DateTime.Now - bg.lastMovement.Value).TotalMinutes;
                        if (waitTime < 5) o.name = "wait";
                        else
                        {
                            o.name = "roam";
                            newLoc = GiveRandomDestination(bg, 250);
                        }
                        break;
                    case "complete_task":
                        o.name = "return";
                        newLoc = GetReturnDestination(bg);
                        break;
                    case "return":
                        newLoc = GetReturnDestination(bg);
                        if (bg.location.Distance(newLoc) < 10)
                        {
                            bg.location = null;
                            bg.orders_id = null;
                            newLoc = null;

                            using (var db = new MinionWarsEntities())
                            {
                                db.Battlegroup.Attach(bg);
                                db.Entry(bg).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        break;
                }
                o.location = newLoc;
                o.directions = Geolocations.Geolocations.GetNewDirections(bg.location, o);
                o.current_step = Geolocations.Geolocations.GetDirectionMovement(bg.location, o);

                using (var db = new MinionWarsEntities())
                {
                    db.Orders.Attach(o);
                    db.Entry(o).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return o;
            }
            else return null;
        }

        private static DbGeography GiveRandomDestination(Battlegroup bg, int distance)
        {
            DbGeography newLoc = null;

            Random rand = new Random();
            double latMove = GenerateRandomDistance(rand, distance);
            double lonMove = GenerateRandomDistance(rand, distance);
            
            var point = string.Format("POINT({1} {0})", (bg.location.Latitude + latMove), (bg.location.Longitude + lonMove));
            newLoc = DbGeography.FromText(point);

            return newLoc;
        }

        private static double GenerateRandomDistance(Random r, int distance)
        {
            int generatedDistance = r.Next(Convert.ToInt32(Math.Floor((decimal)distance / 2)), distance);
            int orientation = r.Next(1, 100);
            if (orientation <= 50) orientation = -1;
            else orientation = 1;

            decimal moveDec = orientation * (decimal)generatedDistance / (1852m * 60m);
            double move = (double)moveDec;

            return move;
        }
        
        private static DbGeography GetReturnDestination(Battlegroup bg)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.UserMovementHistory.Where(x => x.users_id == bg.owner_id).OrderByDescending(x => x.occurence).First().location;
            }
        }
    }
}
