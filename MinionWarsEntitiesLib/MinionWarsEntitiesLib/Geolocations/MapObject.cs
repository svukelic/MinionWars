using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public class MapObject
    {
        public double latitude;
        public double longitude;
        public string type;
        public int id;

        public MapObject(int id, string type, DbGeography loc)
        {
            this.id = id;
            this.type = type;
            this.latitude = loc.Latitude.Value;
            this.longitude = loc.Longitude.Value;
        }
    }
}
