using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Structures
{
    public static class HiveManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();

        public static HiveNode generateRandomHive(int loc)
        {
            Random r = new Random();

            HiveNode newHive = new HiveNode();
            newHive.location_id = loc;
            newHive.minion_id = r.Next(1, db.Minion.ToList().Count());

            return newHive;
        }
    }
}
