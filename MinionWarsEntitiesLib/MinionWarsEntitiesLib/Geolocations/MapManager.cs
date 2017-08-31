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
                camps = db.Camp.Where(x=>x.location.Distance(loc) <= radius && x.owner_id == null).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach(Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, "Neutral Camp", c.location);
                    mdm.objectList.Add(obj);
                }
                camps = null;

                camps = db.Camp.Where(x => x.location.Distance(loc) <= radius && x.owner_id == id).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach (Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, "Your Camp", c.location);
                    mdm.objectList.Add(obj);
                }
                camps = null;

                camps = db.Camp.Where(x => x.location.Distance(loc) <= radius && x.owner_id != null && x.owner_id != id).ToList(); //CampManager.ReturnCamps(loc, radius);
                foreach (Camp c in camps)
                {
                    MapObject obj = new MapObject(c.id, "Player Camp", c.location);
                    mdm.objectList.Add(obj);
                }

                return mdm;
            }
        }
    }
}
