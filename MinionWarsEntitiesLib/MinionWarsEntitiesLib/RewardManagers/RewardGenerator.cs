using MinionWarsEntitiesLib.Combat;
using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.RewardManagers
{
    public static class RewardGenerator
    {
        public static void CombatReward(CombatLog log)
        {
            int exp = 0;

            using (var db = new MinionWarsEntities())
            {
                exp = Convert.ToInt32(db.ModifierCoeficients.Find(24).value);
            }

            if (log.winner.owner_id != null)
            {
                ExperienceManager.IncreaseExperience(log.winner.owner_id.Value, exp);
                Random r = new Random();
                using (var db = new MinionWarsEntities())
                {
                    List<BattlegroupAssignment> ba = db.BattlegroupAssignment.Where(x => x.battlegroup_id == log.loser.id).ToList();
                    int target = r.Next(0, ba.Count);
                    AwardMinions(log.winner.owner_id.Value, ba[target].minion_id, 5);
                }
            }
        }

        public static void AwardMinions(int user_id, int minion_id, int amount)
        {
            using (var db = new MinionWarsEntities())
            {
                MinionOwnership mo = null;
                List<MinionOwnership> list = db.MinionOwnership.Where(x => x.owner_id == user_id && x.minion_id == minion_id).ToList();
                if (list.Count > 0)
                {
                    mo = list.First();
                    mo.group_count += amount;
                    mo.available += amount;

                    db.MinionOwnership.Attach(mo);
                    db.Entry(mo).State = System.Data.Entity.EntityState.Modified;
                }
                else {
                    mo = new MinionOwnership();
                    mo.group_count = 10;
                    mo.available = 10;
                    mo.owner_id = user_id;
                    mo.minion_id = minion_id;

                    db.MinionOwnership.Add(mo);
                }

                db.SaveChanges();
            }
        }
    }
}
