using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class UserDataModel
    {
        public Users userModel;
        public List<MinionModel> minions;
        public BattlegroupModel personalBg;
        public List<BattlegroupModel> remoteBgs;

        public UserDataModel(int id)
        {
            this.userModel = UsersManager.GetUserData(id);

            minions = new List<MinionModel>();
            List<MinionOwnership> ownership = OwnershipManager.GetOwnershipData(id);
            foreach(MinionOwnership mo in ownership)
            {
                Minion m = OwnershipManager.GetMinionsData(mo.minion_id);
                MinionType mt = OwnershipManager.GetTypeData(m.mtype_id);
                MinionModel mm = new MinionModel(m, mo, mt, null);

                minions.Add(mm);
            }

            if (userModel.personal_bg_id != null) personalBg = new BattlegroupModel(userModel.personal_bg_id.Value);
            else userModel.personal_bg_id = -1;

            this.remoteBgs = new List<BattlegroupModel>();
            List<Battlegroup> remote_bgs = OwnershipManager.GetRemoteBattlegroups(userModel.id, userModel.personal_bg_id.Value);
            foreach(Battlegroup b in remote_bgs)
            {
                remoteBgs.Add(new BattlegroupModel(b.id));
            }
        }
    }
}