using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class OrdersParser
    {
        public static string UpdateNextLoc(string directions)
        {
            OrderDirections obj = JsonConvert.DeserializeObject<OrderDirections>(directions);

            obj.last_id = obj.next_id;
            obj.next_id++;
            return JsonConvert.SerializeObject(obj);
        }

        public static DbGeography GetNextLoc(string directions)
        {
            OrderDirections obj = JsonConvert.DeserializeObject<OrderDirections>(directions);
            if (obj.last_id == obj.list.Count) return null;
            else
            {
                string lat = obj.list.Where(x => x.id == obj.next_id).First().lat;
                string lon = obj.list.Where(x => x.id == obj.next_id).First().lon;
                var point = string.Format("POINT({1} {0})", lat, lon);
                DbGeography loc = DbGeography.FromText(point);

                return loc;
            }
        }

        public static string ParseDirections(string result)
        {
            OrderDirections od = new OrderDirections();
            int k = 0;

            dynamic obj = JsonConvert.DeserializeObject(result);
            dynamic route = obj.routes[0];
            for(int i=0; i<route.legs.Count; i++)
            {
                dynamic leg = route.legs[i];
                for(int j=0; j<leg.steps.Count; j++)
                {
                    Directions dir = new Directions(k, (leg.steps[j].end_location.lat).ToString(), (leg.steps[j].end_location.lng).ToString());
                    od.list.Add(dir);
                    if (od.next_id == -1) od.next_id = k;
                    k++;
                }
            }

            return JsonConvert.SerializeObject(od);
        }

        public class OrderDirections
        {
            public int last_id;
            public int next_id;
            public List<Directions> list;

            public OrderDirections()
            {
                this.last_id = -1;
                this.next_id = -1;
                this.list = new List<Directions>();
            }
        }

        public class Directions
        {
            public int id;
            public string lat;
            public string lon;

            public Directions(int id, string lat, string lon)
            {
                this.id = id;
                this.lat = lat;
                this.lon = lon;
            }
        }
    }
}
