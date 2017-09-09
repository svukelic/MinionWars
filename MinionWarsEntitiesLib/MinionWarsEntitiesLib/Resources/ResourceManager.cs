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
                List<ResourceNode> list = db.ResourceNode.Where(x => x.location.Distance(loc) <= 250).ToList();
                if(list.Count > 0)
                {
                    return null;
                }
                else
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

        public static void ConsumeResourceNode(int user_id, int node_id)
        {
            using (var db = new MinionWarsEntities())
            {
                ResourceNode node = db.ResourceNode.Find(node_id);
                UserTreasury ut = db.UserTreasury.Where(x => x.user_id == user_id && x.res_id == node.rtype_id).First();
                ut.amount += 30;

                db.UserTreasury.Attach(ut);
                db.Entry(ut).State = System.Data.Entity.EntityState.Modified;

                db.ResourceNode.Remove(node);

                db.SaveChanges();
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
                    ut.generation = 3;

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
