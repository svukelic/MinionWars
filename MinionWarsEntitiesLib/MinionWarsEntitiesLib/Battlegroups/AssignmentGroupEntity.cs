using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Abilities;
using MinionWarsEntitiesLib.Combat;

namespace MinionWarsEntitiesLib.Battlegroups
{
    public class AssignmentGroupEntity
    {
        public Minion minionData;
        public Ability ability;
        public Ability attack;
        public GroupCombatStats stats;

        public int initialCount;
        public int turnStartCount;
        public int remainingCount;

        public void UseAttack(BattleGroupEntity targetGroup)
        {

        }

        public void UseAbility(BattleGroupEntity targetGroup)
        {

        }

        public void CalculateGroupStats(Battlegroup bg, int line)
        {
            GroupCombatStats stats = new GroupCombatStats();

            stats.strength += Convert.ToInt32(this.minionData.strength + (this.minionData.strength * bg.str_mod));
            stats.dexterity += Convert.ToInt32(this.minionData.dexterity + (this.minionData.dexterity * bg.dex_mod));
            stats.vitality += Convert.ToInt32(this.minionData.vitality + (this.minionData.vitality * bg.vit_mod));
            stats.health = stats.vitality * 5 * this.initialCount;
            //stats.healthPerMinion = stats.vitality * 5;
            stats.power += Convert.ToInt32(this.minionData.power + (this.minionData.power * bg.pow_mod));
            stats.cooldown += Convert.ToInt32(this.minionData.cooldown);
            stats.duration += Convert.ToInt32(this.minionData.duration);
        }
    }
}
