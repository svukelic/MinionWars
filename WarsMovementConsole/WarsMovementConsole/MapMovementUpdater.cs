using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarsMovementConsole
{
    public static class MapMovementUpdater
    {
        public static Battlegroup GetNewAssignment(int lastAssigned)
        {
            Battlegroup newBg = BattlegroupManager.GetLastAssigned(lastAssigned);

            return newBg;
        }

        public static List<Battlegroup> GetAllBattlegroups()
        {
            return BattlegroupManager.GetAllActiveGroups();
        }

        public static List<Caravan> GetAllCaravans()
        {
            return CampManager.GetAllActiveGroups();
        }

        public static Battlegroup UpdateBattlegroupPosition(Battlegroup bg)
        {
            return BattlegroupManager.UpdatePosition(bg);
        }

        public static Caravan UpdateCaravanPosition(Caravan car)
        {
            return CampManager.UpdatePosition(car);
        }
    }
}
