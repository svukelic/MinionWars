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
        public int remainingHealth;

        public void UseAttack(BattleGroupEntity targetGroup)
        {
            attack.effect.PerformEffect(targetGroup, attack, turnStartCount);
        }

        public void UseAbility(BattleGroupEntity targetGroup)
        {
            //use ability

            this.ability.remainingCooldown = this.ability.cooldown;
        }

        public void CalculateGroupStats(Battlegroup bg, int line)
        {
            GroupCombatStats stats = new GroupCombatStats();

            stats.strength += Convert.ToInt32(this.minionData.strength + (this.minionData.strength * bg.str_mod));
            stats.dexterity += Convert.ToInt32(this.minionData.dexterity + (this.minionData.dexterity * bg.dex_mod));
            stats.vitality += Convert.ToInt32(this.minionData.vitality + (this.minionData.vitality * bg.vit_mod));
            stats.health = stats.vitality * 5 * this.initialCount;
            stats.healthPerMinion = stats.vitality * 5;
            stats.power += Convert.ToInt32(this.minionData.power + (this.minionData.power * bg.pow_mod));
            stats.cooldown += Convert.ToInt32(this.minionData.cooldown);
            stats.duration += Convert.ToInt32(this.minionData.duration);

            this.stats = stats;
        }

        public void TakeDamage(int dmg)
        {
            Console.WriteLine("TURN START: " + this.turnStartCount);
            Console.WriteLine("START remainingHealth: " + this.remainingHealth);
            Console.WriteLine("START remainingCount: " + this.remainingCount);

            this.remainingHealth -= dmg;
            if (this.remainingHealth < 0)
            {
                remainingHealth = 0;
                remainingCount = 0;
            }
            else
            {
                this.remainingCount = Convert.ToInt32(Math.Ceiling((decimal)this.remainingHealth / (decimal)this.stats.healthPerMinion));
            }

            Console.WriteLine("dmg: " + dmg);
            Console.WriteLine("health: " + this.stats.healthPerMinion);
            Console.WriteLine("END remainingHealth: " + this.remainingHealth);
            Console.WriteLine("END remainingCount: " + this.remainingCount);
        }
    }
}
