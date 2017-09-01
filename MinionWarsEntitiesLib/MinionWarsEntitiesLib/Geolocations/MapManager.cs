using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Structures;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class MapManager
    {
        public static MapDataModel GetMapData(int id, string point, int radius)
        {
            MapDataModel mdm = new MapDataModel();
            DbGeography loc = DbGeography.FromText(point);

            List<Battlegroup> bg = new List<Battlegroup>();
            List<Users> users = new List<Users>();
            List<Camp> camps = new List<Camp>();
            List<ResourceNode> res = new List<ResourceNode>();
            List<HiveNode> hives = new List<HiveNode>();
            List<Caravan> caravans = new List<Caravan>();
            using (var db = new MinionWarsEntities())
            {
                bg = db.Battlegroup.Where(x => x.location.Distance(loc) <= radius).ToList();
                foreach (Battlegroup b in bg)
                {
                    MapObject obj = new MapObject(b.id, "Battlegroup", b.location);
                    mdm.objectList.Add(obj);
                }

                users = db.Users.Where(x => x.location.Distance(loc) <= radius && x.online == 1).ToList();
                foreach (Users u in users)
                {
                    if(u.id != id)
                    {
                        MapObject obj = new MapObject(u.id, "Player - " + u.username, u.location);
                        mdm.objectList.Add(obj);
                    }
                }

                //camps
                //CampManager.CheckForDiscovery(loc, radius);

                camps = db.Camp.Where(x=>x.location.Distance(loc) <= radius && x.owner_id == null).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach(Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, c.name, c.location);
                    mdm.objectList.Add(obj);
                }
                camps = null;

                camps = db.Camp.Where(x => x.location.Distance(loc) <= radius && x.owner_id == id).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach (Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, "Your Camp - " + c.name, c.location);
                    mdm.objectList.Add(obj);
                }
                camps = null;

                camps = db.Camp.Where(x => x.location.Distance(loc) <= radius && x.owner_id != null && x.owner_id != id).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach (Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, "Player Camp - " + c.name, c.location);
                    mdm.objectList.Add(obj);
                }

                //caravans
                caravans = db.Caravan.Where(x => x.location.Distance(loc) <= radius).ToList();
                foreach (Caravan car in caravans)
                {
                    MapObject obj = new MapObject(car.id, "Caravan", car.location);
                    mdm.objectList.Add(obj);
                }

                //resources
                res = db.ResourceNode.Where(x => x.location.Distance(loc) <= radius).ToList();
                foreach(ResourceNode r in res)
                {
                    MapObject obj = new MapObject(r.id, "Resource Node - " + db.ResourceType.Find(r.rtype_id).name, r.location);
                    mdm.objectList.Add(obj);
                }

                //hives
                hives = db.HiveNode.Where(x => x.location.Distance(loc) <= radius).ToList();
                foreach (HiveNode h in hives)
                {
                    MapObject obj = new MapObject(h.id, "Hive Node", h.location);
                    mdm.objectList.Add(obj);
                }

                return mdm;
            }
        }
    }
}
