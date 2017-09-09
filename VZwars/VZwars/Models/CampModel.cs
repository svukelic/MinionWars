using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class CampModel
    {
        public int id;
        public string name;
        public double lat;
        public double lon;
        public int owner_id;
        public int reputation;

        public CampModel(Camp camp, int id)
        {
            this.id = camp.id;
            this.name = camp.name;
            this.lat = camp.location.Latitude.Value;
            this.lon = camp.location.Longitude.Value;
            if (camp.owner_id != null) this.owner_id = camp.owner_id.Value;
            else this.owner_id = -1;

            List<Reputation> repl = OwnershipManager.GetUserCampReputation(id, camp.id);
            if (repl.Count == 0) this.reputation = 0;
            else this.reputation = repl.First().value.Value;
        }
    }
}