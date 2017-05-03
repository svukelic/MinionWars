using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VZwars.Models.Abilities;

namespace VZwars.Models.Entities
{
    public class MinionType
    {
        public string name;
        public int numericValue;
        public Effect allyEffect;
        public Effect enemyEffect;
        public PassiveEffect passiveEffect;

        public MinionType(int nmbr)
        {
            switch (nmbr)
            {
                case 0:
                    this.name = "";
                    this.numericValue = 0;
                    break;
            }
        }
    }
}