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
        public static MapDataModel GetMapData(string point, int radius)
        {
            MapDataModel mdm = new MapDataModel();
            DbGeography loc = DbGeography.FromText(point);

            List<Battlegroup> bg = new List<Battlegroup>();
            using (var db = new MinionWarsEntities())
            {
                bg = db.Battlegroup.Where(x => x.location.Distance(loc) <= radius).ToList();

                foreach (Battlegroup b in bg)
                {
                    MapObject obj = new MapObject(b.id, "Battlegroup", b.location);
                    mdm.objectList.Add(obj);
                }

                return mdm;
            }
        }
    }
}
