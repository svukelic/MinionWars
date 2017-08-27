using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class BattlegroupModel
    {
        public Battlegroup battlegroup;
        public List<MinionModel> minions;
        public int currentAmount;

        public BattlegroupModel(int id)
        {
            this.battlegroup = OwnershipManager.GetBattlegroupData(id);
            this.minions = new List<MinionModel>();
            this.currentAmount = 0;

            List<BattlegroupAssignment> bga = OwnershipManager.GetAssignmentData(id);
            foreach(BattlegroupAssignment a in bga)
            {
                Minion m = OwnershipManager.GetMinionsData(a.minion_id);
                MinionType mt = OwnershipManager.GetTypeData(m.mtype_id);
                this.minions.Add(new MinionModel(m, null, mt, a));
                this.currentAmount += a.group_count;
            }
        }
    }
}