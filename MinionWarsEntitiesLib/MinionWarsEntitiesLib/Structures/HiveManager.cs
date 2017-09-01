using MinionWarsEntitiesLib.Minions;
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
        public static HiveNode generateRandomHive(DbGeography loc)
        {
            using (var db = new MinionWarsEntities())
            {
                if (!CheckIfHiveExists(loc))
                {
                    HiveNode newHive = new HiveNode();
                    newHive.location = loc;
                    Minion randomMinion = MinionGenotype.generateRandomMinion();
                    newHive.minion_id = randomMinion.id;

                    db.HiveNode.Add(newHive);
                    db.SaveChanges();

                    return newHive;
                }
                else return null;
            }
        }

        private static bool CheckIfHiveExists(DbGeography loc)
        {
            using (var db = new MinionWarsEntities())
            {
                bool exists = false;

                int count = 0;
                count = db.HiveNode.Where(x => x.location.Distance(loc) <= 250).ToList().Count;

                if (count > 0)
                {
                    exists = true;
                }

                return exists;
            }
        }

        public static void ConsumeHiveNode(int user_id, int node_id)
        {
            using (var db = new MinionWarsEntities())
            {
                HiveNode node = db.HiveNode.Find(node_id);
                MinionOwnership mo = null;
                List<MinionOwnership> list = db.MinionOwnership.Where(x => x.owner_id == user_id && x.minion_id == node.minion_id).ToList();
                if (list.Count > 0) {
                    mo = list.First();
                    mo.group_count += 10;
                    mo.available += 10;

                    db.MinionOwnership.Attach(mo);
                    db.Entry(mo).State = System.Data.Entity.EntityState.Modified;
                }
                else {
                    mo = new MinionOwnership();
                    mo.group_count = 10;
                    mo.available = 10;
                    mo.owner_id = user_id;
                    mo.minion_id = node.minion_id;

                    db.MinionOwnership.Add(mo);
                }

                db.HiveNode.Remove(node);
                db.SaveChanges();
            }
        }
    }
}
