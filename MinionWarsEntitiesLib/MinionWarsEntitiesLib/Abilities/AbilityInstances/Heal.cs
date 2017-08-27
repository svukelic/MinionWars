using MinionWarsEntitiesLib.Battlegroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinionWarsEntitiesLib.Abilities.AbilityInstances
{
    public class Heal : Effect
    {
        public override void PerformEffect(BattleGroupEntity targetGroup, Ability abilityData, int count)
        {
            List<AssignmentGroupEntity> targetLine = null;
            if (targetGroup.frontline.Count != 0) targetLine = targetGroup.frontline;
            else if (targetGroup.backline.Count != 0) targetLine = targetGroup.backline;
            else targetLine = targetGroup.supportline;

            Random r = new Random();
            int target = r.Next(0, targetLine.Count - 1);
            targetLine[target].TakeDamage(abilityData.power * count);
        }
    }
}