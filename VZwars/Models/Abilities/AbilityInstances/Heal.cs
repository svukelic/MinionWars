using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Abilities.AbilityInstances
{
    public class Heal : Ability
    {
        public Heal(int type, int build)
        {
            this.name = "heal";
            CalculateAbilityStats(type);
            AssignEffect(build);
        }

        public override void AbilityActivation()
        {

        }

        public override void CalculateAbilityStats(int type)
        {

        }

        public override void AssignEffect(int build)
        {

        }
    }
}