using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Resources
{
    public class CostObject
    {
        public CostsBuilding cost;
        public ResourceType res;

        public CostObject(CostsBuilding c)
        {
            this.cost = c;

            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                this.res = db.ResourceType.Find(c.r_id);
            }
        }
    }
}
