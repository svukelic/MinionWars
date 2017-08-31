using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class ReputationModel
    {
        public Camp camp;
        public Reputation reputation;

        public ReputationModel(Camp camp, Reputation reputation)
        {
            this.camp = camp;
            this.reputation = reputation;
        }
    }
}