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
        public static void SaveToPool(Battlegroup bg)
        {
            using (var db = new MinionWarsEntities())
            {
                EvolutionPool ep = new EvolutionPool();
                Random r = new Random();

                List<BattlegroupAssignment> bga = db.BattlegroupAssignment.Where(x => x.battlegroup_id == bg.id).ToList();
                ep.minion_id = bga.OrderBy(x => r.Next()).First().minion_id;
                ep.last_location = bg.location;
                ep.stored_date = DateTime.Now;

                db.EvolutionPool.Add(ep);
                db.SaveChanges();
            }
        }

        public static void InitiateOntogenesis(DbGeography location)
        {
            using (var db = new MinionWarsEntities())
            {
                List<EvolutionPool> epl = db.EvolutionPool.Where(x => x.last_location.Distance(location) <= 10000).ToList();
                if(epl.Count > 5)
                {
                    GenerateGeneticMinionGroup(location, epl);
                }
                else
                {
                    GenerateWildMinionGroup(location);
                }
            }
        }

        public static void GenerateWildMinionGroup(DbGeography location)
        {
            Random r = new Random();
            Minion WildMinion = MinionGenotype.generateRandomMinion();
            Battlegroup WildGroup = BattlegroupManager.ConstructBattlegroup(null, 0, "Wild Group");
            WildGroup.location = location;

            using (var db = new MinionWarsEntities())
            {
                db.Battlegroup.Add(WildGroup);
                db.SaveChanges();
            }

            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 0, WildGroup);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 1, WildGroup);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 2, WildGroup);

            using (var db = new MinionWarsEntities())
            {
                List<BattlegroupAssignment> bgaList = db.BattlegroupAssignment.Where(x => x.battlegroup_id == WildGroup.id).ToList();
                foreach(BattlegroupAssignment bga in bgaList)
                {
                    BattlegroupManager.CalculateAdvancedModifiers(WildGroup, bga.id, WildMinion.passive);
                }
            }
            //CalculateAdvancedModifiers(bg, count, minion.passive);

            //orders
            Orders o = OrdersManager.GiveNewOrders(WildGroup, "roam", null);
            WildGroup.lastMovement = DateTime.Now;

            using (var db = new MinionWarsEntities())
            {
                WildGroup.orders_id = o.id;
                //db.Battlegroup.Add(WildGroup);
                db.Battlegroup.Attach(WildGroup);
                db.Entry(WildGroup).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void GenerateGeneticMinionGroup(DbGeography location, List<EvolutionPool> epl)
        {
            Random r = new Random();
            epl = epl.OrderBy(x => r.Next()).ToList();
            Minion WildMinion = MinionGenotype.generateGeneticMinion(epl[0], epl[1]);
            Battlegroup WildGroup = BattlegroupManager.ConstructBattlegroup(null, 0, "Wild Group");
            WildGroup.location = location;

            using (var db = new MinionWarsEntities())
            {
                db.Battlegroup.Add(WildGroup);
                db.SaveChanges();
            }

            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 0, WildGroup);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 1, WildGroup);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 2, WildGroup);

            using (var db = new MinionWarsEntities())
            {
                List<BattlegroupAssignment> bgaList = db.BattlegroupAssignment.Where(x => x.battlegroup_id == WildGroup.id).ToList();
                foreach (BattlegroupAssignment bga in bgaList)
                {
                    BattlegroupManager.CalculateAdvancedModifiers(WildGroup, bga.id, WildMinion.passive);
                }
            }
            //CalculateAdvancedModifiers(bg, count, minion.passive);

            //orders
            Orders o = OrdersManager.GiveNewOrders(WildGroup, "roam", null);
            WildGroup.lastMovement = DateTime.Now;

            using (var db = new MinionWarsEntities())
            {
                WildGroup.orders_id = o.id;
                //db.Battlegroup.Add(WildGroup);
                db.Battlegroup.Attach(WildGroup);
                db.Entry(WildGroup).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
