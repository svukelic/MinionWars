using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Geolocations;
using MinionWarsEntitiesLib.Minions;
using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Resources;

namespace WarsGeneratorConsole
{
    class Program
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        static List<Location> pendingLocEvents = new List<Location>();
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
            List<UserMovementHistory> umh = db.UserMovementHistory.Where(x => (x.occurence - DateTime.Now).TotalMinutes <= 15).ToList();
            foreach(UserMovementHistory u in umh)
            {
                Location loc = db.Location.Find(u.location_id);
                if(loc.activity_saturation == 0.5 || loc.activity_saturation == 0.9)
                {
                    pendingLocEvents.Add(loc);
                }
                //pendingLocEvents.Add(db.Location.Where(x => x.activity_saturation == 0.5 || x.activity_saturation == 0.9));
            }
        }

        private static async Task AssignEvents()
        {
            while (true)
            {
                if(pendingLocEvents.Count > 0)
                {
                    Location currentLoc = pendingLocEvents.First();

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

        private static void ResGenerationEvent(Location loc)
        {
            //generate resource node
            ResourceNode res = ResourceManager.generateRandomResource(loc.id);
        }

        private static void MinionGenerationEvent(Location loc)
        {
            //generate wild minion group
            Random r = new Random();
            Minion WildMinion = MinionGenotype.generateRandomMinion();
            Battlegroup WildGroup = BattlegroupManager.ConstructBattlegroup(null, 1);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 0, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 1, WildGroup.id);
            BattlegroupManager.AddMinions(WildMinion.id, r.Next(2, 13), 2, WildGroup.id);

            //increase event saturation, 500 m, +100 on loc, 100 - (distance / 5) on locs within 500 m
            if (loc.event_saturation == null) loc.event_saturation = 0;
            loc.event_saturation += 100;

            List<Location> nearbyLocations = db.Location.Where(x => Geolocations.GetDistance(x, loc) <= 500).ToList();
            foreach (Location l in nearbyLocations)
            {
                if (l.event_saturation == null) l.event_saturation = 0;
                l.event_saturation += (100 - Geolocations.GetDistance(loc, l) / 5);
            }

        }

        private static void HiveGenerationEvent(object source, ElapsedEventArgs e)
        {
            //generate new hives in top 10% event saturated locs, lower event saturation
        }
    }
}
