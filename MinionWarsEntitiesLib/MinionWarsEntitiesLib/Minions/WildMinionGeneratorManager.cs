using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Minions
{
    public static class WildMinionGeneratorManager
    {
        public static void GenerateWildMinionGroup()
        {
            Random r = new Random();
            Minion WildMinion = MinionGenotype.generateRandomMinion();
            Battlegroup WildGroup = BattlegroupManager.ConstructBattlegroup(null, 1);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 0, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 1, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 2, WildGroup.id);
        }
    }
}
