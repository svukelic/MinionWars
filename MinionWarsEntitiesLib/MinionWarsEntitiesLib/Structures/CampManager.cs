using MinionWarsEntitiesLib.Geolocations;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Structures
{
    public static class CampManager
    {
        public static List<Camp> ReturnCamps(DbGeography loc, int radius)
        {
            using (var db = new MinionWarsEntities())
            {
                List<Camp> camps = db.Camp.Where(x => x.location.Distance(loc) <= radius).ToList();
                //Check for discovery - max 16, check if at least 10
                if (camps.Count <= 10)
                {
                    camps.AddRange(Geolocations.Geolocations.InitiateDiscovery(loc)); 
                }

                return camps;
            }
        }

        public static void CheckForDiscovery(DbGeography loc, int radius)
        {
            using (var db = new MinionWarsEntities())
            {
                List<Camp> camps = db.Camp.Where(x => x.location.Distance(loc) <= radius).ToList();
                //Check for discovery - max 16, check if at least 10
                if (camps.Count <= 10)
                {
                    camps.AddRange(Geolocations.Geolocations.InitiateDiscovery(loc));
                }
                camps = null;
            }
        }

        public static Camp GetCampInfo(int camp_id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Camp.Find(camp_id);
            }
        }

        public static Camp CreateCamp(int user_id, DbGeography loc, string name)
        {
            using (var db = new MinionWarsEntities())
            {
                Camp camp = new Camp();
                if (user_id == -1)
                {
                    camp.owner_id = null;
                    camp.type = "neutral";
                    camp.name = "Neutral Camp";
                    camp.size = 30;
                    camp.building_count = 3;

                    bool result = CostManager.ApplyCampCost(camp.owner_id.Value);
                    if (!result) return null;
                }
                else {
                    camp.owner_id = user_id;
                    camp.type = "owned";
                    camp.name = name;
                    camp.size = CalculateCampCapacity(user_id);
                    camp.building_count = 0;
                }
                camp.location = loc;
                camp.mapped_type = "restaurant";
                camp.richness = 0;
                camp.def_modifier = 0;
                camp.bg_id = null;

                db.Camp.Add(camp);
                db.SaveChanges();

                if (user_id == -1)
                {
                    List<ResourceType> res = ResourceManager.getRandomRes();
                    for (int i = 0; i < 2; i++)
                    {
                        CampTreasury ct = new CampTreasury();
                        ct.camp_id = camp.id;
                        ct.amount = 100;
                        ct.res_id = res[i].id;
                        db.CampTreasury.Add(ct);

                        ResourceBuilding rb = new ResourceBuilding();
                        Buildings building = db.Buildings.Find(res[i].id); //db.Buildings.Where(x => x.name.Contains(res[i].name.ToLower())).First();
                        rb.camp_id = camp.id;
                        rb.name = building.name;
                        rb.rtype_id = res[i].id;
                        db.ResourceBuilding.Add(rb);
                    }

                    UtilityBuilding ub = new UtilityBuilding();
                    Buildings b2 = db.Buildings.Find(16);
                    ub.camp_id = camp.id;
                    ub.name = b2.name;
                    ub.type = 2;
                    ub.description = null;
                    db.UtilityBuilding.Add(ub);

                    db.SaveChanges();
                }

                return camp;
            }
        }

        public static List<Caravan> GenerateCaravans()
        {
            using (var db = new MinionWarsEntities())
            {
                List<Caravan> list = new List<Caravan>();
                Random r = new Random();
                List<Camp> camps = db.Camp.Where(x => x.owner_id == null).ToList();
                camps = camps.OrderBy(a => r.Next()).ToList();

                int count = Convert.ToInt32(Math.Floor((decimal)camps.Count / 10));
                if (count == 0) count = 1;

                for(int i=0; i< count; i++)
                {
                    Caravan caravan = new Caravan();
                    caravan.owner_id = null;
                    caravan.source_id = camps[i].id;
                    caravan.location = camps[i].location;
                    caravan.last_movement = DateTime.Now;

                    List<Camp> nearbyCamps = db.Camp.Where(x => x.location.Distance(caravan.location) <= 2000).ToList();
                    Camp destinationCamp = nearbyCamps[r.Next(0, nearbyCamps.Count)];
                    caravan.destination_id = destinationCamp.id;

                    caravan.directions = Geolocations.Geolocations.GetCaravanDirections(caravan.location, destinationCamp.location);

                    list.Add(caravan);
                    db.Caravan.Add(caravan);
                    db.SaveChanges();
                }

                return list;
            }
        }

        public static Caravan CaravanArrival(Caravan car)
        {
            car.current_step = null;
            car.location = null;
            car.directions = null;

            using (var db = new MinionWarsEntities())
            {
                Camp source = db.Camp.Find(car.source_id);
                Camp destination = db.Camp.Find(car.destination_id);

                source.richness++;
                destination.richness++;

                if(source.owner_id != null)
                {
                    Reputation rep = null;
                    List<Reputation> list = db.Reputation.Where(x => x.user_id == source.owner_id && x.camp_id == destination.id).ToList();
                    if(list.Count > 0)
                    {
                        rep = list.First();
                        rep.value++;

                        db.Reputation.Attach(rep);
                        db.Entry(rep).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        rep = new Reputation();
                        rep.value = 1;

                        db.Reputation.Add(rep);
                    }
                }

                db.Caravan.Attach(car);
                db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                db.Camp.Attach(source);
                db.Entry(source).State = System.Data.Entity.EntityState.Modified;
                db.Camp.Attach(destination);
                db.Entry(destination).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }

            return car;
        }

        public static List<Caravan> GetAllActiveGroups()
        {
            using (var db = new MinionWarsEntities())
            {
                List<Caravan> carl = db.Caravan.Where(x => x.location != null).ToList();
                return carl;
            }
        }

        public static Caravan UpdatePosition(Caravan car)
        {
            return PositionManager.UpdateCaravanPosition(car);
        }

        public static Camp CreateUserCamp(string point, int owner_id, string name)
        {
            DbGeography loc = DbGeography.FromText(point);
            using (var db = new MinionWarsEntities())
            {
                Camp camp = null;

                Users owner = db.Users.Find(owner_id);
                List<Camp> ownedCamps = db.Camp.Where(x => x.owner_id == owner_id).ToList();
                if(ownedCamps.Count < owner.trait_architecture + 1)
                {
                    List<Camp> list = db.Camp.Where(x => x.location.Distance(loc) <= 250).ToList();
                    if (list.Count == 0) camp = CreateCamp(owner_id, loc, name);
                }

                return camp;
            }
        }

        public static bool DestroyCamp(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                Camp camp = db.Camp.Find(id);

                db.Camp.Remove(camp);
                db.SaveChanges();

                return true;
            }
        }

        public static string ConsumeCaravan(int user_id, int car_id)
        {
            string res = "";

            using (var db = new MinionWarsEntities())
            {
                Caravan car = db.Caravan.Find(car_id);
                Camp camp = db.Camp.Find(car.source_id);
                Camp destination = db.Camp.Find(car.destination_id);

                Random r = new Random();
                List<CampTreasury> ctl = db.CampTreasury.Where(x => x.camp_id == camp.id).ToList();
                CampTreasury ct = ctl.OrderBy(x => r.Next()).First();
                ResourceType rt = db.ResourceType.Find(ct.res_id);

                res = rt.name;

                UserTreasury ut = db.UserTreasury.Where(x => x.user_id == user_id && x.res_id == rt.id).First();
                ut.amount += 30;

                db.UserTreasury.Attach(ut);
                db.Entry(ut).State = System.Data.Entity.EntityState.Modified;

                camp.richness -= 1;
                destination.richness -= 1;

                db.Camp.Attach(camp);
                db.Entry(camp).State = System.Data.Entity.EntityState.Modified;
                db.Camp.Attach(destination);
                db.Entry(destination).State = System.Data.Entity.EntityState.Modified;

                db.Caravan.Remove(car);

                db.SaveChanges();
            }

            return res;
        }

        private static int CalculateCampCapacity(int user_id)
        {
            using (var db = new MinionWarsEntities())
            {
                Users user = db.Users.Find(user_id);

                return 20 + user.trait_architecture * 2;
            }
        }

        public static List<Buildings> GetAllBuildings()
        {
            using (var db = new MinionWarsEntities())
            {
                return db.Buildings.ToList();
            }
        }

        public static bool CreateBuilding(int camp_id, int b_id)
        {
            using (var db = new MinionWarsEntities())
            {
                Camp camp = db.Camp.Find(camp_id);
                Buildings b = db.Buildings.Find(b_id);

                bool result = CostManager.ApplyBuildingCosts(b.id, camp.owner_id.Value);
                if (!result) return false;

                switch (b.type)
                {
                    case "Resource Building":
                        ResourceBuilding rb = new ResourceBuilding();
                        rb.name = b.name;
                        rb.camp_id = camp_id;
                        rb.rtype_id = b.id;
                        db.ResourceBuilding.Add(rb);
                        break;
                    case "Offensive Building":
                        OffensiveBuilding ob = new OffensiveBuilding();
                        ob.name = b.name;
                        ob.minion_id = null;
                        ob.camp_id = camp_id;
                        db.OffensiveBuilding.Add(ob);
                        break;
                    case "Defensive Building":
                        DefensiveBuilding defb = new DefensiveBuilding();
                        defb.camp_id = camp_id;
                        if(b_id == 14)
                        {
                            camp.def_modifier += Int32.Parse(b.description);
                        }
                        else if(b_id == 17)
                        {
                            camp.size += Int32.Parse(b.description);
                        }
                        db.DefensiveBuilding.Add(defb);
                        break;
                    case "Utility Building":
                        UtilityBuilding ub = new UtilityBuilding();
                        ub.camp_id = camp_id;
                        ub.name = b.name;
                        ub.type = b_id;
                        ub.description = "";
                        db.UtilityBuilding.Add(ub);
                        break;
                }

                camp.building_count++;
                db.Camp.Attach(camp);
                db.Entry(camp).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return true;
            }
        }

        public static void DestroyBuilding(int camp_id, int b_id, int type)
        {
            /*
            * 1 - resource
            * 2 - offensive
            * 3 - defensive
            * 4 - utility
            */

            using (var db = new MinionWarsEntities())
            {
                switch (type)
                {
                    case 1:
                        db.ResourceBuilding.Remove(db.ResourceBuilding.Find(b_id));
                        break;
                    case 2:
                        db.OffensiveBuilding.Remove(db.OffensiveBuilding.Find(b_id));
                        break;
                    case 3:
                        db.DefensiveBuilding.Remove(db.DefensiveBuilding.Find(b_id));
                        break;
                    case 4:
                        db.UtilityBuilding.Remove(db.UtilityBuilding.Find(b_id));
                        break;
                }

                Camp camp = db.Camp.Find(camp_id);
                camp.building_count--;
                db.Camp.Attach(camp);
                db.Entry(camp).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }
        }
    }
}
