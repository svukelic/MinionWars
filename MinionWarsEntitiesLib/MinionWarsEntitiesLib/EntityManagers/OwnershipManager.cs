using MinionWarsEntitiesLib.Battlegroups;
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
                db.Configuration.LazyLoadingEnabled = false;
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
                db.Configuration.LazyLoadingEnabled = false;
                Minion m = new Minion();
                m = db.Minion.Find(id);

                return m;
            }
        }

        public static MinionType GetTypeData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.MinionType.Find(id);
            }
        }

        public static Battlegroup GetBattlegroupData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Battlegroup.Find(id);
            }
        }

        public static List<BattlegroupAssignment> GetAssignmentData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.BattlegroupAssignment.Where(x => x.battlegroup_id == id).ToList();
            }
        }

        public static List<Battlegroup> GetRemoteBattlegroups(int id, int personal_id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Battlegroup.Where(x => x.owner_id == id && x.type != 1).ToList();
            }
        }

        public static List<Camp> GetUserCamps(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<Camp> list = db.Camp.Where(x => x.owner_id == id).ToList();
                foreach(Camp c in list)
                {
                    c.ResourceBuilding = db.ResourceBuilding.Where(x => x.camp_id == c.id).ToList();
                    c.OffensiveBuilding = db.OffensiveBuilding.Where(x => x.camp_id == c.id).ToList();
                    c.DefensiveBuilding = db.DefensiveBuilding.Where(x => x.camp_id == c.id).ToList();
                    c.UtilityBuilding = db.UtilityBuilding.Where(x => x.camp_id == c.id).ToList();
                }
                return list;
            }
        }

        public static List<Reputation> GetUserReputation(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Reputation.Where(x => x.user_id == id).ToList();
            }
        }

        public static AbilityStats GetAbilityData(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.AbilityStats.Find(id);
            }
        }

        public static string ProcessAddition(int? o_id, int? amount, int? line, int? bg_id, string name)
        {
            if (amount == null) return "amount is null";
            else if (line == null) return "line is null";
            else if (o_id == null) return "ownership is null";
            else if (bg_id == null && (name == null || name == "")) return "bg error";
            else
            {
                Battlegroup bg = null;
                MinionOwnership mo = null;
                using (var db = new MinionWarsEntities())
                {
                    mo = db.MinionOwnership.Find(o_id);
                    if (mo.available < amount) return "Too much minions";
                    else
                    {
                        if (name != null && name != "")
                        {
                            bg = BattlegroupManager.ConstructBattlegroup(mo.owner_id, 2, name);
                            db.Battlegroup.Add(bg);
                        }
                        else if (bg_id != null)
                        {
                            bg = db.Battlegroup.Find(bg_id.Value);
                        }

                        mo.available -= amount;
                        db.MinionOwnership.Attach(mo);
                        db.Entry(mo).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                    }
                }
                if(bg != null && mo != null) BattlegroupManager.AddMinions(mo.minion_id, amount.Value, line.Value, bg);
            }

            return "ok";
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

        public static void CreateNewPersonalBattlegroup(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                Battlegroup bg = BattlegroupManager.ConstructBattlegroup(id, 1, "Personal Battlegroup");
                Users user = db.Users.Find(id);

                db.Battlegroup.Add(bg);
                db.SaveChanges();

                user.personal_bg_id = bg.id;

                db.Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
