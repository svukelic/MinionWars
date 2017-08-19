using MinionWarsEntitiesLib.AiManagers;
using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Minions
{
    public static class WildMinionGeneratorManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static void GenerateWildMinionGroup(DbGeography location)
        {
            Random r = new Random();
            Minion WildMinion = MinionGenotype.generateRandomMinion();
            Battlegroup WildGroup = BattlegroupManager.ConstructBattlegroup(null, 1);
            WildGroup.location = location;
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 0, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 1, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 2, WildGroup.id);
            //db.SaveChanges();

            //orders
            Orders o = OrdersManager.GiveNewOrders(WildGroup, "roam", null);

            WildGroup.orders_id = o.id;
            db.Battlegroup.Add(WildGroup);
            //db.Battlegroup.Attach(WildGroup);
            //db.Entry(WildGroup).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
