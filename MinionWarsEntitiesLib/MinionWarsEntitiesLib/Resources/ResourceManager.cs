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
        public static ResourceNode generateRandomResource(DbGeography loc)
        {
            using (var db = new MinionWarsEntities())
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

        public static List<ResourceType> getRandomRes()
        {
            using (var db = new MinionWarsEntities())
            {
                Random r = new Random();
                List<ResourceType> list = db.ResourceType.ToList();
                list = list.OrderBy(a => r.Next()).ToList();
                return list;
            }
        }

        public static void GenerateNewUserResources(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                List<ResourceType> list = db.ResourceType.ToList();
                foreach(ResourceType rt in list)
                {
                    UserTreasury ut = new UserTreasury();
                    ut.user_id = id;
                    ut.res_id = rt.id;
                    ut.amount = 50;

                    db.UserTreasury.Add(ut);
                }
                db.SaveChanges();
            }
        }

        public static List<UserTreasury> GetUserTreasury(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserTreasury> ut = db.UserTreasury.Where(x => x.user_id == id).ToList();
                foreach(UserTreasury u in ut)
                {
                    u.ResourceType = db.ResourceType.Find(u.res_id);
                }

                return ut;
            }
        }
    }
}
