using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Resources;
using MinionWarsEntitiesLib.Structures;
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
        public List<TreasuryModel> treasury;
        public List<Camp> personalCamps;
        public List<ReputationModel> reputation;
        public List<Buildings> buildings;

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

            this.treasury = new List<TreasuryModel>();
            List<UserTreasury> ut = ResourceManager.GetUserTreasury(userModel.id);
            foreach(UserTreasury u in ut)
            {
                TreasuryModel tm = new TreasuryModel(u.ResourceType.name, u.amount.Value, u.ResourceType.category);
                this.treasury.Add(tm);
            }

            this.personalCamps = OwnershipManager.GetUserCamps(id);

            List<Reputation> repList = OwnershipManager.GetUserReputation(id);
            foreach(Reputation r in repList)
            {
                Camp c = CampManager.GetCampInfo(r.camp_id);
                ReputationModel rm = new ReputationModel(c, r);
                this.reputation.Add(rm);
            }

            this.buildings = CampManager.GetAllBuildings();
        }
    }
}