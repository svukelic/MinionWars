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

namespace WarsGeneratorConsole
{
    class Program
    {
        static List<DbGeography> pendingLocEvents = new List<DbGeography>();
        static Random r = new Random();
        static void Main(string[] args)
        {
            Timer locTimer = new Timer(1000 * 60 * 15);
            locTimer.Elapsed += new ElapsedEventHandler(GetLocs);

            AssignEvents();

            Timer hiveTimer = new Timer(1000 * 60 * 60);
            hiveTimer.Elapsed += new ElapsedEventHandler(HiveGenerationEvent);

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void GetLocs(object source, ElapsedEventArgs e)
        {
            List<UserMovementHistory> umh = UsersManager.GetLatestLocations(15);
            foreach(UserMovementHistory u in umh)
            {
                if(u.activity_saturation.Value == 0.5 || u.activity_saturation.Value == 0.9)
                {
                    pendingLocEvents.Add(u.location);
                }
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

        private static void ResGenerationEvent(DbGeography loc)
        {
            //generate resource node
            ResourceNode res = ResourceManager.generateRandomResource(loc);
            UsersManager.UpdateEventSaturations(loc, 1);
        }

        private static void MinionGenerationEvent(DbGeography loc)
        {
            //generate wild minion group
            WildMinionGeneratorManager.GenerateWildMinionGroup();
            UsersManager.UpdateEventSaturations(loc, 1);
        }

        private static void HiveGenerationEvent(object source, ElapsedEventArgs e)
        {
            //generate new hives in top 10% event saturated locs, lower event saturation
            List<UserMovementHistory> umh = UsersManager.GetHighestEventSaturationLocations(10);
            foreach(UserMovementHistory u in umh)
            {
                HiveManager.generateRandomHive(u.location);
                UsersManager.UpdateEventSaturations(u.location, -5);
            }
        }
    }
}
