using MinionWarsEntitiesLib.Minions;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public class OwnershipManager
    {
        public static List<MinionOwnership> GetOwnershipData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.MinionOwnership.Where(x => x.owner_id == id).ToList();
            }
        }

        public static List<Minion> GetMinionsData(List<MinionOwnership> moList)
        {
            using (var db = new MinionWarsEntities())
            {
                List<Minion> minions = new List<Minion>();

                foreach(MinionOwnership mo in moList)
                {
                    Minion m = new Minion();
                    m = db.Minion.Find(mo.minion_id);
                    minions.Add(m);
                }

                return minions;
            }
        }

        public static Minion GetMinionsData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                Minion m = new Minion();
                m = db.Minion.Find(id);

                return m;
            }
        }

        public static MinionType GetTypeData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.MinionType.Find(id);
            }
        }

        public static Battlegroup GetBattlegroupData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.Battlegroup.Find(id);
            }
        }

        public static List<BattlegroupAssignment> GetAssignmentData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.BattlegroupAssignment.Where(x => x.battlegroup_id == id).ToList();
            }
        }

        public static List<Battlegroup> GetRemoteBattlegroups(int id, int personal_id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.Battlegroup.Where(x => x.owner_id == id && x.id != personal_id).ToList();
            }
        }

        public static AbilityStats GetAbilityData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.AbilityStats.Find(id);
            }
        }

        public static void GenerateNewUserOwnership(int id)
        {
            for(int i = 0; i<3; i++)
            {
                using (var db = new MinionWarsEntities())
                {
                    Minion minion = MinionGenotype.generateRandomMinion();

                    MinionOwnership mo = new MinionOwnership();
                    mo.owner_id = id;
                    mo.group_count = 10;
                    mo.minion_id = minion.id;
                    mo.available = 10;

                    db.MinionOwnership.Add(mo);
                    db.SaveChanges();
                }
            }
        }
    }
}
