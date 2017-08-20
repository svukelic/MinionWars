using MinionWarsEntitiesLib.Models;
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
                        MapObject obj = new MapObject(u.id, u.username, u.location);
                        mdm.objectList.Add(obj);
                    }
                }

                return mdm;
            }
        }
    }
}
