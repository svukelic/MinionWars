using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Combat
{
    public class CombatLog
    {
        public Battlegroup winner;
        public Battlegroup loser;
        public List<string> log;

        public void SaveLog()
        {
            //using db
        }
    }
}
