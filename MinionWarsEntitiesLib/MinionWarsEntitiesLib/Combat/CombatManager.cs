using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Battlegroups;

namespace MinionWarsEntitiesLib.Combat
{
    public static class CombatManager
    {
        public static Battlegroup StartCombat(int bg1_id, int bg2_id)
        {
            BattleGroupEntity bge1 = BattlegroupManager.BuildBattleGroupEntity(bg1_id);
            BattleGroupEntity bge2 = BattlegroupManager.BuildBattleGroupEntity(bg2_id);

            return PerformCombat(bge1, bge2);
        }

        private static Battlegroup PerformCombat(BattleGroupEntity bge1, BattleGroupEntity bge2)
        {
            bool combatEnd = false;
            Battlegroup winner = null;

            while(!combatEnd) {
                //turn start
                TurnStartCountAdjustement(bge1);
                TurnStartCountAdjustement(bge2);

                //front
                CalculateLinePerformance(bge1.frontline, bge2);
                CalculateLinePerformance(bge2.frontline, bge1);

                //back
                CalculateLinePerformance(bge1.backline, bge2);
                CalculateLinePerformance(bge2.backline, bge1);

                //support
                CalculateLinePerformance(bge1.supportline, bge2);
                CalculateLinePerformance(bge2.supportline, bge1);

                if (CheckIfGroupIsDead(bge1))
                {
                    combatEnd = !combatEnd;
                    winner = bge2.bg;
                }
                else if (CheckIfGroupIsDead(bge2))
                {
                    combatEnd = !combatEnd;
                    winner = bge1.bg;
                }
            }

            return winner;
        }

        private static void CalculateLinePerformance(List<AssignmentGroupEntity> attacker, BattleGroupEntity target)
        {
            foreach (AssignmentGroupEntity age in attacker)
            {
                if (age.ability.remainingCooldown == 0) age.UseAbility(target);
                else age.UseAttack(target);
            }
        }

        private static void TurnStartCountAdjustement(BattleGroupEntity bge)
        {
            foreach (AssignmentGroupEntity age in bge.frontline)
            {
                age.turnStartCount = age.remainingCount;
            }

            foreach (AssignmentGroupEntity age in bge.backline)
            {
                age.turnStartCount = age.remainingCount;
            }

            foreach (AssignmentGroupEntity age in bge.supportline)
            {
                age.turnStartCount = age.remainingCount;
            }
        }

        private static bool CheckIfLineIsDead(List<AssignmentGroupEntity> line)
        {
            bool isDead = true;

            foreach (AssignmentGroupEntity age in line)
            {
                if (age.remainingCount > 0) isDead = false;
            }

            return isDead;
        }

        private static bool CheckIfGroupIsDead(BattleGroupEntity bge)
        {
            bool isDead = false;

            if (CheckIfLineIsDead(bge.frontline) && CheckIfLineIsDead(bge.backline) && CheckIfLineIsDead(bge.supportline)) isDead = true;

            return isDead;
        }
    }
}
