using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Combat
{
    public class GroupCombatStats
    {
        public int strength;
        public int dexterity;

        public int vitality;
        public int health;
        public int healthPerMinion;
        public int shield;

        public int power;
        public int duration;
        public int cooldown;

        public GroupCombatStats()
        {
            this.strength = 0;
            this.dexterity = 0;
            this.vitality = 0;
            this.health = 0;
            this.healthPerMinion = 0;
            this.power = 0;
            this.duration = 0;
            this.cooldown = 0;

            this.shield = 0;
        }
    }
}
