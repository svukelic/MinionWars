using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VZwars.Models.Abilities.AbilityInstances;

namespace VZwars.Models.Abilities
{
    public static class AbilityGenerator
    {
        public static Ability GenerateAbility(int nmbr, int type, int build)
        {
            Ability ability;

            switch (nmbr)
            {
                case 1:
                    ability = new Heal(type, build);
                    break;
                default:
                    ability = null;
                    break;
            }

            return ability;
        }
    }
}