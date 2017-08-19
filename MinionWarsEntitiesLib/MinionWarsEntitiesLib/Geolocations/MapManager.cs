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
        static MinionWarsEntities db = new MinionWarsEntities();
        public static MapDataModel GetMapData(string point, int radius)
        {
            MapDataModel mdm = new MapDataModel();
            DbGeography loc = DbGeography.FromText(point);

            //mdm.userList.AddRange(db.Users.Where(x => x.lo)
            mdm.bgList.AddRange(db.Battlegroup.Where(x => x.location.Distance(loc) <= radius));

            return mdm;
        }
    }
}
