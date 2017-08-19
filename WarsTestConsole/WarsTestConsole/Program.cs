using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib;
using MinionWarsEntitiesLib.TestManagers;
using MinionWarsEntitiesLib.Geolocations;

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
            MapDataModel mdm = MapManager.GetMapData(point, 1000);

            Console.WriteLine(mdm.bgList.Count);
            //WildMinionsTest.Generate(point);
        }
    }
}
