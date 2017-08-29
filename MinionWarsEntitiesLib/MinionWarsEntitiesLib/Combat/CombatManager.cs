using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.RewardManagers;

namespace MinionWarsEntitiesLib.Combat
{
    public static class CombatManager
    {
        public static CombatLog StartCombat(int bg1_id, int bg2_id)
        {
            if (CheckForOwnership(bg1_id, bg2_id))
            {
                BattleGroupEntity bge1 = BattlegroupManager.BuildBattleGroupEntity(bg1_id);
                BattleGroupEntity bge2 = BattlegroupManager.BuildBattleGroupEntity(bg2_id);

                CombatLog log = PerformCombat(bge1, bge2);

                return log;
            }
            else return null;
        }

        private static CombatLog PerformCombat(BattleGroupEntity bge1, BattleGroupEntity bge2)
        {
            CombatLog log = new CombatLog();
            bool combatEnd = false;

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
                    combatEnd = true;
                    log.winner = bge2.bg;
                    log.loser = bge1.bg;
                }
                else if (CheckIfGroupIsDead(bge2))
                {
                    combatEnd = true;
                    log.winner = bge1.bg;
                    log.loser = bge2.bg;
                }
            }

            log.SaveLog();
            RewardGenerator.CombatReward(log);

            return log;
        }

        private static void CalculateLinePerformance(List<AssignmentGroupEntity> attacker, BattleGroupEntity target)
        {
            foreach (AssignmentGroupEntity age in attacker)
            {
                //if (age.ability.remainingCooldown == 0) age.UseAbility(target);
                //else age.UseAttack(target);
                age.UseAttack(target);
            }
        }

        private static void TurnStartCountAdjustement(BattleGroupEntity bge)
        {
            //int totalCount = 0;

            foreach (AssignmentGroupEntity age in bge.frontline)
            {
                age.turnStartCount = age.remainingCount;
                //totalCount += age.turnStartCount;
            }

            foreach (AssignmentGroupEntity age in bge.backline)
            {
                age.turnStartCount = age.remainingCount;
                //totalCount += age.turnStartCount;
            }

            foreach (AssignmentGroupEntity age in bge.supportline)
            {
                age.turnStartCount = age.remainingCount;
                //totalCount += age.turnStartCount;
            }

            //Console.WriteLine("Turn start (" + bge.bg.id + "): " + totalCount);
        }

        public static bool CheckIfLineIsDead(List<AssignmentGroupEntity> line)
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

        private static bool CheckForOwnership(int bg1_id, int bg2_id)
        {
            using (var db = new MinionWarsEntities())
            {
                Battlegroup bg1 = db.Battlegroup.Find(bg1_id);
                Battlegroup bg2 = db.Battlegroup.Find(bg2_id);

                if (bg1.owner_id == bg2.owner_id) return false;
                else return true;
            }
        }
    }
}
