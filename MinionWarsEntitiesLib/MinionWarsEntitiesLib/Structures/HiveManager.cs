using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Structures
{
    public static class HiveManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();

        public static HiveNode generateRandomHive(DbGeography loc)
        {
            if (!CheckIfHiveExists(loc))
            {
                Random r = new Random();

                HiveNode newHive = new HiveNode();
                newHive.location = loc;
                newHive.minion_id = r.Next(1, db.Minion.ToList().Count());

                db.HiveNode.Add(newHive);
                db.SaveChanges();

                return newHive;
            }
            else return null;
        }

        private static bool CheckIfHiveExists(DbGeography loc)
        {
            bool exists = false;

            int count = 0;
            count = db.HiveNode.Where(x => x.location.Distance(loc) <= 250).ToList().Count;

            if(count > 0)
            {
                exists = true;
            }

            return exists;
        }
    }
}
