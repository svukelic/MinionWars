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
            /*DbGeography loc = DbGeography.FromText(point);
            MapDataModel mdm = MapManager.GetMapData(point, 1000);
            Console.WriteLine(mdm.objectList.Count);*/

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

            CombatLog log = CombatManager.StartCombat(32, 34);
            Console.WriteLine("winner: " + log.winner.id);

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}
