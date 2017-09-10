using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MinionWarsEntitiesLib.EntityManagers;
using System.Data.Entity.Spatial;
using MinionWarsEntitiesLib.Resources;
using MinionWarsEntitiesLib.Minions;
using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Structures;
using MinionWarsEntitiesLib.RewardManagers;

namespace WarsGeneratorConsole
{
    class Program
    {
        static List<DbGeography> pendingLocEvents = new List<DbGeography>();
        static Random r = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            Timer satTimer = new Timer(1000 * 60 * 5);
            satTimer.Elapsed += new ElapsedEventHandler(UpdateActivitySaturation);
            satTimer.Enabled = true;

            Timer locTimer = new Timer(1000 * 60 * 15);
            locTimer.Elapsed += new ElapsedEventHandler(GetLocs);
            locTimer.Enabled = true;

            AssignEvents();

            Timer hiveTimer = new Timer(1000 * 60 * 60);
            hiveTimer.Elapsed += new ElapsedEventHandler(HiveGenerationEvent);
            hiveTimer.Enabled = true;

            Timer caravanTimer = new Timer(1000 * 60 * 15);
            caravanTimer.Elapsed += new ElapsedEventHandler(CaravanGenerationEvent);
            hiveTimer.Enabled = true;

            Timer tTimer = new Timer(1000 * 60 * 60);
            tTimer.Elapsed += new ElapsedEventHandler(UpdateUserTreasuries);
            tTimer.Enabled = true;

            while (true)
            {
                Console.WriteLine(DateTime.Now + " - Pending events count: " + pendingLocEvents.Count);
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void GetLocs(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("GetLocs initiated");
            List<UserMovementHistory> umh = UsersManager.GetLatestLocations(60);
            if(umh.Count > 0)
            {
                //Console.WriteLine("Movement count: " + umh.Count);
                foreach (UserMovementHistory u in umh)
                {
                    decimal value = (decimal)u.activity_saturation.Value;
                    if ((value >= (decimal)0.45 && value <= (decimal)0.55) || (value >= (decimal)0.85 && value <= (decimal)0.95))
                    {
                        pendingLocEvents.Add(u.location);
                    }
                }
            }
            else
            {
                Console.WriteLine("No recent movement");
            }
        }

        private static async Task AssignEvents()
        {
            while (true)
            {
                if(pendingLocEvents.Count > 0)
                {
                    DbGeography currentLoc = pendingLocEvents.First();

                    if(r.Next(0,1) == 0)
                    {
                        ResGenerationEvent(currentLoc);
                    }
                    else
                    {
                        MinionGenerationEvent(currentLoc);
                    }

                    pendingLocEvents.Remove(currentLoc);
                }

                await Task.Delay(100);
            }
        }

        private static void UpdateActivitySaturation(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Activity saturation update started");
            UsersManager.UpdateActivitySaturations();
        }

        private static void ResGenerationEvent(DbGeography loc)
        {
            Console.WriteLine("Resource generation started");
            //generate resource node
            ResourceNode res = ResourceManager.generateRandomResource(loc);
            UsersManager.UpdateEventSaturations(loc, 1);
            Console.WriteLine("Resource generated: " + res.location.Latitude + " | " + res.location.Longitude);
        }

        private static void MinionGenerationEvent(DbGeography loc)
        {
            Console.WriteLine("Ontogenesis started");
            //generate wild minion group
            WildMinionGeneratorManager.InitiateOntogenesis(loc);
            UsersManager.UpdateEventSaturations(loc, 1);
        }

        private static void HiveGenerationEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Hive generation started");
            //generate new hives in top 10% event saturated locs, lower event saturation
            List<UserMovementHistory> umh = UsersManager.GetHighestEventSaturationLocations((decimal)0.1);
            foreach(UserMovementHistory u in umh)
            {
                HiveNode h = HiveManager.generateRandomHive(u.location);
                UsersManager.UpdateEventSaturations(u.location, -5);
                Console.WriteLine("Hive generated: " + h.location.Latitude + " | " + h.location.Longitude);
            }
        }

        private static void CaravanGenerationEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Caravan generation started");
            List<Caravan> list = CampManager.GenerateCaravans();
            foreach(Caravan c in list)
            {
                Console.WriteLine("Caravan generated: " + c.location.Latitude + " | " + c.location.Longitude);
            }
        }

        private static void UpdateUserTreasuries(object source, ElapsedEventArgs e)
        {
            TreasuryUpdater.UpdateUserTreasuries();
        }
    }
}
