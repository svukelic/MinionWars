using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class MinionModel
    {
        public Minion data;
        public MinionOwnership ownership;
        public MinionType type;
        public AbilityStats m_ability;
        public AbilityStats r_ability;

        public BattlegroupAssignment assignment;

        public MinionModel(Minion data, MinionOwnership ownership, MinionType type, BattlegroupAssignment ba)
        {
            this.data = data;
            this.ownership = ownership;
            this.type = type;
            this.assignment = ba;
            //this.m_ability = OwnershipManager.GetAbilityData(this.data.melee_ability);
            //this.r_ability = OwnershipManager.GetAbilityData(this.data.ranged_ability);
        }
    }
}