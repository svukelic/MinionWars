using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class CaravanDisplayModel
    {
        public int type;
        public int source;
        public Caravan caravan;
        public List<CampModel> camps;

        public CaravanDisplayModel(int type, int source, Caravan car, List<Camp> cl, int id)
        {
            this.type = type;
            this.source = source;
            this.caravan = car;

            camps = new List<CampModel>();
            foreach (Camp c in cl)
            {
                if(id != c.owner_id) camps.Add(new CampModel(c, id));
            }
        }
    }
}