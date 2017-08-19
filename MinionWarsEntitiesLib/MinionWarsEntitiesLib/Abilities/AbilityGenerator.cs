using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinionWarsEntitiesLib.Abilities.AbilityInstances;
using MinionWarsEntitiesLib.Minions;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Combat;
using MinionWarsEntitiesLib.Abilities.AttackInstances;

namespace MinionWarsEntitiesLib.Abilities
{
    public static class AbilityGenerator
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static Ability GenerateAbility(int line, GroupCombatStats gcs, Minion m)
        {
            Ability ability = new Ability();

            int abilityDesignation = 0;
            if (line == 1) abilityDesignation = m.melee_ability;
            else abilityDesignation = m.ranged_ability;

            GetAbilityStats(ability, gcs, abilityDesignation);

            switch (abilityDesignation)
            {
                case 1:
                    ability.effect = new Heal();
                    break;
                default:
                    ability = null;
                    break;
            }

            return ability;
        }

        public static Ability GenerateAttack(int line, GroupCombatStats gcs, Minion m)
        {
            Ability ability = new Ability();
            int attackDesignation = 0;

            if (line == 1)
            {
                attackDesignation = 1;
                ability.effect = new MeleeAttack();
                GetAttackStats(ability, gcs, attackDesignation);
            }
            else if (line == 2)
            {
                attackDesignation = 2;
                ability.effect = new MeleeAttack();
                GetAttackStats(ability, gcs, attackDesignation);
            }
            else
            {
                GetAbilityStats(ability, gcs, m.melee_ability);
            }

            return ability;
        }

        //ability calculations
        private static void GetAttackStats(Ability a, GroupCombatStats gcs, int attackDesignation)
        {
            AbilityStats ast = db.AbilityStats.Find(attackDesignation);
            a.name = ast.name;
            a.power = ast.power;
            a.cooldown = 0; // ast.cooldown;
            a.remainingCooldown = 0; // ast.cooldown;

            int powerModifier = 0;

            if (attackDesignation == 1) powerModifier = gcs.strength;
            else powerModifier = gcs.dexterity;

            a.power += powerModifier;
        }

        private static void GetAbilityStats(Ability a, GroupCombatStats gcs, int attackDesignation)
        {
            AbilityStats ast = db.AbilityStats.Find(attackDesignation);
            a.name = ast.name;
            a.power = ast.power;
            a.cooldown = ast.cooldown;
            a.duration = ast.duration;

            a.power += gcs.power;
            a.cooldown += gcs.cooldown;
            a.duration += gcs.duration;

            a.remainingCooldown = a.cooldown;
        }

    }
}