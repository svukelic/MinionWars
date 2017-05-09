using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinionWarsEntitiesLib.Abilities.AbilityInstances;
using MinionWarsEntitiesLib.Minions;
using MinionWarsEntitiesLib.Models;

namespace MinionWarsEntitiesLib.Abilities
{
    public static class AbilityGenerator
    {
        public static Ability GenerateAbility(int nmbr, Minion m)
        {
            Ability ability = null;

            switch (nmbr)
            {
                case 1:
                    //ability = new Heal(type, build);
                    break;
                default:
                    ability = null;
                    break;
            }

            return ability;
        }
    }
}