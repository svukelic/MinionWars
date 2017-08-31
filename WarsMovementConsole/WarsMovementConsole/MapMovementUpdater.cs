﻿using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Models;
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

        public static List<Battlegroup> GetAll()
        {
            return BattlegroupManager.GetAllActiveGroups();
        }

        public static Battlegroup UpdatePosition(Battlegroup bg)
        {
            return BattlegroupManager.UpdatePosition(bg);
        }
    }
}
