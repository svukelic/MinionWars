using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib;

namespace WarsTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("TEST");
            Console.WriteLine("Ispis: " + MinionWarsEntitiesLib.EntityManagers.UsersManager.TestData());
        }
    }
}
