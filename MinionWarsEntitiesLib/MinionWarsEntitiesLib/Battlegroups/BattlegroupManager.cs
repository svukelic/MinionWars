using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Abilities;
using System.Data.Entity.Spatial;
using MinionWarsEntitiesLib.AiManagers;
using MinionWarsEntitiesLib.Geolocations;

namespace MinionWarsEntitiesLib.Battlegroups
{
    public static class BattlegroupManager
    {
        public static Battlegroup ConstructBattlegroup(int? owner_id, int type, string name)
        {
            Battlegroup bg = new Battlegroup();
            SetBasicModifiers(bg);
            bg.creation = DateTime.Now;

            if (owner_id != null)
            {
                using(var db = new MinionWarsEntities())
                {
                    Users owner = db.Users.Find(owner_id);
                    if (owner != null)
                    {
                        bg.owner_id = owner_id.Value;

                        /* types
                        * 1 - personal
                        * 2 - remote
                        * 3 - defensive
                        */
                        bg.type = type;

                        bg.movement_mod += owner.trait_logistics * 0.1;

                        int sizeModifier = 0;
                        if (type == 1)
                        {
                            sizeModifier = 20 + owner.trait_leadership * 3;
                            bg.loot_mod += owner.trait_leadership * 5;
                            bg.res_mod += owner.trait_leadership * 5;
                            bg.resurrection_mod += owner.trait_leadership;
                        }
                        else if(type == 2)
                        {
                            sizeModifier = 10 + owner.trait_logistics * 2;
                            bg.loot_mod += owner.trait_logistics * 2.5;
                            bg.res_mod += owner.trait_logistics * 2.5;
                            bg.resurrection_mod += owner.trait_logistics;
                        }
                        else if(type == 3)
                        {
                            sizeModifier = 20 + owner.trait_architecture * 2;
                            bg.defense_mod += owner.trait_architecture;
                            bg.build_mod += owner.trait_architecture * 2;
                        }
                        bg.size = 0 + sizeModifier;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                bg.owner_id = null;
            }

            bg.name = name;

            return bg;
        }

        public static Battlegroup AddMinions(int m_id, int count, int type, Battlegroup bg)
        {
            using (var db = new MinionWarsEntities())
            {
                Minion minion = db.Minion.Find(m_id);
                if (minion != null && bg != null)
                {
                    BattlegroupAssignment assignment = new BattlegroupAssignment();

                    assignment.battlegroup_id = bg.id;
                    assignment.minion_id = minion.id;
                    assignment.group_count = count;
                    assignment.line = type;
                    db.BattlegroupAssignment.Add(assignment);

                    /*CalculateAdvancedModifiers(bg, count, minion.passive);

                    db.Battlegroup.Attach(bg);
                    db.Entry(bg).State = System.Data.Entity.EntityState.Modified;*/

                    db.SaveChanges();
                }
                else
                {
                    return null;
                }

                return bg;
            }
        }

        public static bool SendRemoteGroup(int bg_id, string point, string currentPoint)
        {
            Battlegroup bg = null;
            using (var db = new MinionWarsEntities())
            {
                bg = db.Battlegroup.Find(bg_id);
            }
            DbGeography loc = DbGeography.FromText(point);
            DbGeography currentLoc = DbGeography.FromText(currentPoint);
            Orders o = OrdersManager.GiveNewOrders(bg, "complete_task", loc);

            using (var db = new MinionWarsEntities())
            {
                bg.orders_id = o.id;
                bg.location = currentLoc;
                db.Battlegroup.Attach(bg);
                db.Entry(bg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        public static bool RemoveMinions(int a_id)
        {
            using (var db = new MinionWarsEntities())
            {
                BattlegroupAssignment ba = db.BattlegroupAssignment.Find(a_id);
                if (ba != null)
                {
                    MinionOwnership mo = null;
                    Battlegroup bg = db.Battlegroup.Find(ba.battlegroup_id);
                    mo = db.MinionOwnership.Where(x => x.minion_id == ba.minion_id && x.owner_id == bg.owner_id).First();
                    mo.available += ba.group_count;

                    db.BattlegroupAssignment.Remove(ba);
                    db.MinionOwnership.Attach(mo);
                    db.Entry(mo).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                else return false;
            }
        }

        public static Battlegroup GetLastAssigned(int lastAssigned)
        {
            Battlegroup newBg = null;

            while(newBg == null)
            {
                using (var db = new MinionWarsEntities())
                {
                    do
                    {
                        newBg = db.Battlegroup.Find(lastAssigned);
                        lastAssigned++;
                    } while (newBg.location == null || newBg.orders_id == null);
                }
            }

            return newBg;
        }

        public static List<Battlegroup> GetAllActiveGroups()
        {
            using (var db = new MinionWarsEntities())
            {
                List<Battlegroup> bgl = db.Battlegroup.Where(x => x.location != null && x.orders_id != null).ToList();
                return bgl;
            }
        }

        public static Battlegroup UpdatePosition(Battlegroup bg)
        {
            return PositionManager.UpdatePosition(bg);
        }

        /*private static void GetTraitModifiers(Users owner, Battlegroup bg, int type)
        {
            using (var db = new MinionWarsEntities())
            {
                UserTraits traits = db.UserTraits.Find(owner.traits_id);

                if (traits != null)
                {
                    bg.movement_mod += traits.cbg_speed * 0.1;

                    int sizeModifier = 0;
                    if (type == 0) sizeModifier = 20 + traits.pbg_size * 4;
                    else sizeModifier = 10 + traits.cbg_size * 2;
                    bg.size += sizeModifier;

                    bg.build_mod += traits.arch_speed * 0.1;
                }
            }
        }*/

        private static void SetBasicModifiers(Battlegroup bg)
        {
            bg.str_mod = 0;
            bg.dex_mod = 0;
            bg.vit_mod = 0;
            bg.pow_mod = 0;

            bg.res_mod = 0;
            bg.metal_mod = 0;
            bg.stone_mod = 0;
            bg.tree_mod = 0;
            bg.food_mod = 0;
            bg.build_mod = 0;
            bg.movement_mod = 0;
            bg.reproduction_mod = 0;
            bg.loot_mod = 0;

            bg.regen_mod = 0;
            bg.resurrection_mod = 0;
            bg.defense_mod = 0;
        }

        public static void CalculateAdvancedModifiers(Battlegroup bg, int count, int passive)
        {
            switch (passive)
            {
                case 0:
                    bg.str_mod += GetModifierCoeficients(3) * count;
                    break;
                case 1:
                    bg.dex_mod += GetModifierCoeficients(1) * count;
                    break;
                case 2:
                    bg.vit_mod += GetModifierCoeficients(2) * count;
                    break;
                case 3:
                    bg.pow_mod += GetModifierCoeficients(7) * count;
                    break;
                case 4:
                    bg.res_mod += GetModifierCoeficients(4) * count;
                    break;
                case 5:
                    bg.metal_mod += GetModifierCoeficients(13) * count;
                    break;
                case 6:
                    bg.stone_mod += GetModifierCoeficients(9) * count;
                    break;
                case 7:
                    bg.tree_mod += GetModifierCoeficients(10) * count;
                    break;
                case 8:
                    bg.food_mod += GetModifierCoeficients(15) * count;
                    break;
                case 9:
                    bg.build_mod += GetModifierCoeficients(12) * count;
                    break;
                case 10:
                    bg.movement_mod += GetModifierCoeficients(16) * count;
                    break;
                case 11:
                    bg.reproduction_mod += GetModifierCoeficients(14) * count;
                    break;
                case 12:
                    bg.loot_mod += GetModifierCoeficients(6) * count;
                    break;
                case 13:
                    bg.regen_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(11) * count));
                    break;
                case 14:
                    bg.resurrection_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(5) * count));
                    break;
                case 15:
                    bg.defense_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(17) * count));
                    break;
            }

            bg.group_speed = Convert.ToInt32(Math.Floor(10 + 10 * bg.movement_mod));
        }

        private static double GetModifierCoeficients(int id)
        {
            using (var db = new MinionWarsEntities())
            {
                return db.ModifierCoeficients.Find(id).value;
            }
        }

        public static BattleGroupEntity BuildBattleGroupEntity(int bg_id)
        {
            using (var db = new MinionWarsEntities())
            {
                BattleGroupEntity bge = new BattleGroupEntity();

                bge.bg = db.Battlegroup.Find(bg_id);
                if (bge.bg != null)
                {
                    bge.frontline = new List<AssignmentGroupEntity>();
                    bge.backline = new List<AssignmentGroupEntity>();
                    bge.supportline = new List<AssignmentGroupEntity>();

                    List<BattlegroupAssignment> assignments = db.BattlegroupAssignment.Where(x => x.battlegroup_id == bg_id).ToList();
                    if (assignments != null)
                    {
                        foreach (BattlegroupAssignment a in assignments)
                        {
                            AssignmentGroupEntity age = new AssignmentGroupEntity();
                            age.initialCount = a.group_count;
                            age.remainingCount = a.group_count;
                            age.turnStartCount = a.group_count;
                            age.minionData = db.Minion.Find(a.minion_id);
                            age.CalculateGroupStats(bge.bg, a.line);
                            age.remainingHealth = age.stats.health;
                            age.attack = AbilityGenerator.GenerateAttack(a.line, age.stats, age.minionData);
                            //age.ability = AbilityGenerator.GenerateAbility(a.line, age.stats, age.minionData);

                            switch (a.line)
                            {
                                case 0:
                                    bge.frontline.Add(age);
                                    break;
                                case 1:
                                    bge.backline.Add(age);
                                    break;
                                case 2:
                                    bge.supportline.Add(age);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }

                return bge;
            }
        }
    }
}
