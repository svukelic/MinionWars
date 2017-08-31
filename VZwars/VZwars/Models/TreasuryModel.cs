using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class TreasuryModel
    {
        public string name;
        public int amount;
        public string category;

        public TreasuryModel(string name, int amount, string category)
        {
            this.name = name;
            this.amount = amount;
            this.category = category;
        }
    }
}