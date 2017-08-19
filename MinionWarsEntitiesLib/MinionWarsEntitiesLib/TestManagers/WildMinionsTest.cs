using MinionWarsEntitiesLib.Minions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.TestManagers
{
    public static class WildMinionsTest
    {
        public static void Generate(string point)
        {
            DbGeography loc = DbGeography.FromText(point);
            WildMinionGeneratorManager.GenerateWildMinionGroup(loc);
        }
    }
}
