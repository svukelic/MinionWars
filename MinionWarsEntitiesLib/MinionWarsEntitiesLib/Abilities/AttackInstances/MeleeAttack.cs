using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Abilities.AttackInstances
{
    public class MeleeAttack : Effect
    {
        public override void PerformEffect(BattleGroupEntity targetGroup, Ability abilityData, int count)
        {
            List<AssignmentGroupEntity> targetLine = null;
            if (!CombatManager.CheckIfLineIsDead(targetGroup.frontline))
            {
                targetLine = targetGroup.frontline;
            }
            else if (!CombatManager.CheckIfLineIsDead(targetGroup.backline))
            {
                targetLine = targetGroup.backline;
            }
            else if (!CombatManager.CheckIfLineIsDead(targetGroup.supportline))
            {
                targetLine = targetGroup.supportline;
            }
            else return;

            List<int> usedTargets = new List<int>();

            if(targetLine.Count > 0)
            {
                Random r = new Random();
                int target = -1;

                do
                {
                    target = r.Next(0, targetLine.Count);
                } while (targetLine[target].turnStartCount == 0);

                targetLine[target].TakeDamage(abilityData.power * count);
            }
        }
    }
}
