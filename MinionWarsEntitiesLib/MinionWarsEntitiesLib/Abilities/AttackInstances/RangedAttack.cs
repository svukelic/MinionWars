using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Abilities.AttackInstances
{
    public class RangedAttack : Effect
    {
        public override void PerformEffect(BattleGroupEntity targetGroup, Ability abilityData, int count)
        {
            List<AssignmentGroupEntity> targetLine = new List<AssignmentGroupEntity>();
            if (!CombatManager.CheckIfLineIsDead(targetGroup.frontline))
            {
                targetLine.AddRange(targetGroup.frontline);
                Console.WriteLine("Target R: FRONT");
            }
            else if(!CombatManager.CheckIfLineIsDead(targetGroup.supportline))
            {
                targetLine.AddRange(targetGroup.supportline);
                Console.WriteLine("Target R: SUPPORT");
            }

            if (!CombatManager.CheckIfLineIsDead(targetGroup.backline))
            {
                targetLine.AddRange(targetGroup.backline);
                Console.WriteLine("Target R: BACK");
            }

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
