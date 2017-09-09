using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class TradeModel
    {
        public int id;
        public int amount;
        public string mtype;
        public string owner;

        public TradeModel(Trading t)
        {
            this.id = t.id;
            this.amount = t.amount;
            this.mtype = t.Minion.MinionType.name;
            this.owner = t.Users.username;
        }
    }
}