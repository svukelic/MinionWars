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
                }
                else {
                    camp.owner_id = user_id;
                    camp.type = "owned";
                    camp.name = name;
                }
                camp.location = loc;
                camp.mapped_type = "restaurant";
                camp.richness = 0;

                db.Camp.Add(camp);
                db.SaveChanges();

                List<ResourceType> res = ResourceManager.getRandomRes();
                for(int i=0; i<2; i++)
                {
                    CampTreasury ct = new CampTreasury();
                    ct.camp_id = camp.id;
                    ct.amount = 100;
                    ct.res_id = res[i].id;

                    db.CampTreasury.Add(ct);
                }
                db.SaveChanges();

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

                    List<Camp> nearbyCamps = ReturnCamps(camps[i].location, 2000);
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

        private static int CalculateCampCapacity()
        {
            return 0;
        }
    }
}
