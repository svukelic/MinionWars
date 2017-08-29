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

            if (log.winner.owner_id != null) ExperienceManager.IncreaseExperience(log.winner.owner_id.Value, exp);
        }
    }
}
