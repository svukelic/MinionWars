using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;

namespace MinionWarsEntitiesLib.Resources
{
    public static class ResourceManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static ResourceNode generateRandomResource(int loc)
        {
            Random r = new Random();
            List<ResourceType> rtypeList = db.ResourceType.ToList();

            ResourceNode newRes = new ResourceNode();
            newRes.location_id = loc;
            newRes.rtype_id = r.Next(1, rtypeList.Count);

            db.ResourceNode.Add(newRes);
            db.SaveChanges();

            return newRes;
        }
    }
}
