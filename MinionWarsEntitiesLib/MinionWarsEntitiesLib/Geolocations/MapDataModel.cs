using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public class MapDataModel
    {
        public List<MapObject> objectList;

        public MapDataModel()
        {
            this.objectList = new List<MapObject>();
        }
    }
}
