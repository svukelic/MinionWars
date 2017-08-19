using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinionWarsEntitiesLib.Abilities
{
    public class Ability
    {
        public string name;
        public int power;
        public int duration;
        public int cooldown;
        public int remainingCooldown;

        public Effect effect;

        public void AbilityActivation()
        {
            effect.PerformEffect(this);
        }

        /*public Effect effect;
        public int precision;
        abstract public void AbilityActivation();
        abstract public void CalculateAbilityStats(int type);
        abstract public void AssignEffect(int build);*/
    }
}