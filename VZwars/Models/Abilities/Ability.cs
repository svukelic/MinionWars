using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Abilities
{
    public abstract class Ability
    {
        public string name;
        public int power;
        public int precision;
        public int cooldown;
        public int remainingCooldown;

        public Effect effect;

        abstract public void AbilityActivation();
        abstract public void CalculateAbilityStats(int type);
        abstract public void AssignEffect(int build);
    }
}