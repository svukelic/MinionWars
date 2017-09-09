using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Resources
{
    public static class CostManager
    {
        public static bool ApplyBuildingCosts(int b_id, int user_id)
        {
            using (var db = new MinionWarsEntities())
            {
                List<CostsBuilding> cbl = db.CostsBuilding.Where(x => x.b_id == b_id).ToList();
                List<UserTreasury> utl = db.UserTreasury.Where(x => x.user_id == user_id).ToList();
                foreach (CostsBuilding cb in cbl)
                {
                    UserTreasury ut = utl.Where(x => x.res_id == cb.r_id).First();
                    if (cb.amount.Value > ut.amount) return false;
                    else
                    {
                        ut.amount -= cb.amount.Value;
                        db.UserTreasury.Attach(ut);
                        db.Entry(ut).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();

                return true;
            }
        }

        public static bool ApplyMinionCosts(int user_id, int amount)
        {
            using (var db = new MinionWarsEntities())
            {
                List<UserTreasury> utl = db.UserTreasury.Where(x => x.user_id == user_id).ToList();
                foreach (UserTreasury ut in utl)
                {
                    ResourceType rt = db.ResourceType.Find(ut.res_id);
                    if (rt.category.Equals("food"))
                    {
                        if (ut.amount < amount * 20) return false;
                        else
                        {
                            ut.amount -= amount * 20;
                            db.UserTreasury.Attach(ut);
                            db.Entry(ut).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }

                db.SaveChanges();

                return true;
            }
        }

        public static List<CostObject> GetBuildingCosts(int b_id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<CostsBuilding> cbl = db.CostsBuilding.Where(x => x.b_id == b_id).ToList();
                List<CostObject> col = new List<CostObject>();
                foreach(CostsBuilding cb in cbl)
                {
                    CostObject co = new CostObject(cb);
                    col.Add(co);
                }

                return col;
            }
        }

        public static List<CostObject> GetCampCosts()
        {
            List<CostObject> col = new List<CostObject>();

            using (var db = new MinionWarsEntities())
            {
                List<ResourceType> rtl = db.ResourceType.ToList();
                foreach(ResourceType rt in rtl)
                {
                    CostsBuilding cb = new CostsBuilding();
                    cb.r_id = rt.id;
                    cb.amount = 50;

                    CostObject co = new CostObject(cb);
                    col.Add(co);
                }
            }

            return col;
        }
        
        public static bool ApplyCampCost(int user_id)
        {
            using (var db = new MinionWarsEntities())
            {
                List<CostObject> col = GetCampCosts();
                List<UserTreasury> utl = db.UserTreasury.Where(x => x.user_id == user_id).ToList();
                foreach (CostObject co in col)
                {
                    UserTreasury ut = utl.Where(x => x.res_id == co.cost.r_id).First();
                    if (co.cost.amount.Value > ut.amount) return false;
                    else
                    {
                        ut.amount -= co.cost.amount.Value;
                        db.UserTreasury.Attach(ut);
                        db.Entry(ut).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();

                return true;
            }
        }
    }
}
