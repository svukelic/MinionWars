using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WarsMovementConsole
{
    class Program
    {
        static int nextAssignment = 0;
        static int lastAssigned = 0;
        static List<Battlegroup>[] assignedBattlegroups = new List<Battlegroup>[20];
        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            for (int i = 0; i < 20; i++)
            {
                assignedBattlegroups[i] = new List<Battlegroup>();
            }

            AssignBattlegroups();

            for (int i = 0; i < 20; i++)
            {
                DoWork(i);
            }

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private static async Task DoWork(int i)
        {
            while (true)
            {
                //Console.WriteLine("Test - " + i);
                //Console.WriteLine("Count: " + assignedBattlegroups[i].Count);
                foreach (Battlegroup bg in assignedBattlegroups[i])
                {
                    if (MapMovementUpdater.UpdatePosition(bg))
                    {
                        //assignedBattlegroups[i].Remove(bg);
                    }
                    Console.WriteLine("BG POSITION: " + bg.location.ToString());
                }

                /*if (i == 19) nextAssignment = 0;
                else nextAssignment++;*/

                await Task.Delay(100);
            }
        }

        private static async Task AssignBattlegroups()
        {
            while (true)
            {
                Console.WriteLine("Test - " + nextAssignment);
                Battlegroup newBg = MapMovementUpdater.GetNewAssignment(lastAssigned);
                if (newBg != null)
                {
                    assignedBattlegroups[nextAssignment].Add(newBg);
                    lastAssigned = newBg.id + 1;

                    //test
                    //Console.WriteLine("BG FOUND " + "[" + nextAssignment + "]: " + newBg.id);
                }

                if (nextAssignment == 19) nextAssignment = 0;
                else nextAssignment++;

                await Task.Delay(500);
            }
        }
    }
}
