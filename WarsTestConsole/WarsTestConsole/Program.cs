using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib;
using MinionWarsEntitiesLib.TestManagers;
using MinionWarsEntitiesLib.Geolocations;
using MinionWarsEntitiesLib.EntityManagers;
using System.Data.Entity.Spatial;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Combat;
using MinionWarsEntitiesLib.Structures;
using Newtonsoft.Json;
using System.IO;
using MinionWarsEntitiesLib.Resources;
using MinionWarsEntitiesLib.Minions;

namespace WarsTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("TEST");
            //46.318565799999995 16.34576590000006
            var point = string.Format("POINT({1} {0})", 46.31856579999999, 16.34576590000006);
            DbGeography loc = DbGeography.FromText(point);
            /*MapDataModel mdm = MapManager.GetMapData(11, point, 1000);
            Console.WriteLine(mdm.objectList.Count);
            foreach(MapObject m in mdm.objectList)
            {
                Console.WriteLine(m.type);
            }*/

            /*HiveNode h = HiveManager.generateRandomHive(loc);
            UsersManager.UpdateEventSaturations(loc, -5);
            Console.WriteLine("Hive generated: " + h.location.Latitude + " | " + h.location.Longitude);*/

            //UsersManager.UpdateActivitySaturations();

            /*List<Caravan> list = CampManager.GenerateCaravans();
            foreach (Caravan c in list)
            {
                Console.WriteLine("Caravan generated: " + c.location.Latitude + " | " + c.location.Longitude);
            }*/

            //Console.WriteLine(UserDataManager.GetUserData(1).user.username);

            /*var point2 = string.Format("POINT({1} {0})", 46.31856579999999, 17.34576590000006);
            DbGeography l1 = DbGeography.FromText(point);
            DbGeography l2 = DbGeography.FromText(point2);
            Console.WriteLine(l1.ToString());
            Console.WriteLine(l1.Distance(l2));*/

            //Console.WriteLine(UserDataManager.GetUserData(1).user.username);
            //WildMinionsTest.Generate(point);

            /*decimal movement = 1000m / (1852m * 60m);
            double m = (double)movement;
            Console.WriteLine("MOVEMENT: " + m);*/

            /*CombatLog log = CombatManager.StartCombat(32, 34);
            Console.WriteLine("winner: " + log.winner.id);*/

            //Console.WriteLine(StructuresManager.GetPlaces(46.31856579999999, 16.34576590000006, 5000, "restaurant").Result);
            /*string test = Geolocations.GetDirections(46.31856579999999, 16.34576590000006, 46.310833627601156, 16.332077980041504).Result;
            string directions = OrdersParser.ParseDirections(test);
            Console.WriteLine(OrdersParser.ParseNextLoc(directions));*/

            /*dynamic ds = JsonConvert.DeserializeObject(test);
            Console.WriteLine(ds.routes[0].legs[0].steps.Count);
            Console.WriteLine(ds.routes[0].legs[0].steps[0].end_location.lat);*/

            /*List<Camp> camps = CampManager.ReturnCamps(loc, 1000);
            Console.WriteLine(camps.Count);*/

            //CampManager.CheckForDiscovery(loc, 1000);
            //CampManager.CreateCamp(17, loc, "Throne");

            /*string places = Geolocations.GetPlaces(loc.Latitude.Value, loc.Longitude.Value, 1000, "restaurant").Result;
            dynamic obj = JsonConvert.DeserializeObject(places);
            Console.WriteLine(obj.results.Count);
            for (int i = 0; i < obj.results.Count; i++)
            {
                dynamic place = obj.results[i];
                var p = string.Format("POINT({1} {0})", place.geometry.location.lat, place.geometry.location.lng);
                Console.WriteLine("p: " + p);
            }*/

            /*List<UserTreasury> ut = ResourceManager.GetUserTreasury(8);
            Console.WriteLine(ut[0].ResourceType.name);
            Console.WriteLine(ut[0].amount);*/

            using (var db = new MinionWarsEntities())
            {
                List<CostsBuilding> cbl = db.CostsBuilding.Where(x => x.b_id == 1).ToList();
                List<UserTreasury> utl = db.UserTreasury.Where(x => x.user_id == 17).ToList();

                foreach (CostsBuilding cb in cbl)
                {
                    UserTreasury ut = utl.Where(x => x.res_id == cb.r_id).First();
                    Console.WriteLine("Amount: " + cb.amount.Value);
                }

                Console.Write("ok");
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
