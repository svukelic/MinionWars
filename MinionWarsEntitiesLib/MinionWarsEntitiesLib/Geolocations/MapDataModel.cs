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
        public List<Battlegroup> bgList;
        public List<Users> userList;

        public MapDataModel()
        {
            this.bgList = new List<Battlegroup>();
            this.userList = new List<Users>();
        }
    }
}
