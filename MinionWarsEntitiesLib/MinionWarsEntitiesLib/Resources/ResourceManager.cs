using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;
using System.Data.Entity.Spatial;

namespace MinionWarsEntitiesLib.Resources
{
    public static class ResourceManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static ResourceNode generateRandomResource(DbGeography loc)
        {
            Random r = new Random();
            List<ResourceType> rtypeList = db.ResourceType.ToList();

            ResourceNode newRes = new ResourceNode();
            //var point = string.Format("POINT({1} {0})", latitude, longitude);
            newRes.location = loc;
            newRes.rtype_id = r.Next(1, rtypeList.Count);

            db.ResourceNode.Add(newRes);
            db.SaveChanges();

            return newRes;
        }
    }
}
