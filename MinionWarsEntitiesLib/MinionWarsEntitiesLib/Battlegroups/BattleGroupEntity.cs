using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;

namespace MinionWarsEntitiesLib.Battlegroups
{
    public class BattleGroupEntity
    {
        public Battlegroup bg;
        public List<AssignmentGroupEntity> frontline;
        public List<AssignmentGroupEntity> backline;
        public List<AssignmentGroupEntity> supportline;
    }
}
