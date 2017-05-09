using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Battlegroups
{
    public static class BattlegroupManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static Battlegroup ConstructBattlegroup(int? owner_id, int type)
        {
            MinionWarsEntitiesLib.Models.Battlegroup bg = new MinionWarsEntitiesLib.Models.Battlegroup();
            SetBasicModifiers(bg);

            if (owner_id != null)
            {
                Users owner = db.Users.Find(owner_id);
                if (owner != null)
                {
                    bg.owner_id = owner_id.Value;
                    GetTraitModifiers(owner, bg, type);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                bg.owner_id = null;
            }

            db.Battlegroup.Add(bg);
            db.SaveChanges();

            return bg;
        }

        public static bool AddMinions(int m_id, int count, int type, int bg_id)
        {
            Minion minion = db.Minion.Find(m_id);
            MinionWarsEntitiesLib.Models.Battlegroup bg = db.Battlegroup.Find(bg_id);
            if (minion != null && bg != null)
            {
                BattlegroupAssignment assignment = new BattlegroupAssignment();

                assignment.battlegroup_id = bg_id;
                assignment.minion_id = minion.id;
                assignment.group_count = count;
                assignment.line = type;
                db.BattlegroupAssignment.Add(assignment);

                CalculateAdvancedModifiers(bg, count, minion.passive);
                
                db.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        public static Battlegroup GetLastAssigned(int lastAssigned)
        {
            Battlegroup newBg = null;

            while(newBg == null)
            {
                newBg = db.Battlegroup.Find(lastAssigned);
                lastAssigned++;
            }

            return newBg;
        }

        public static bool UpdatePosition(Battlegroup bg)
        {
            Location currentLoc = db.Location.Find(bg.current_loc_id);
            Location destinationLoc = db.Location.Find(bg.destination_loc_id);
            bool arrived = false;

            if(Geolocations.Geolocations.GetDistance(currentLoc, destinationLoc) <= 10)
            {
                arrived = true;
            }
            else
            {
                Location newLoc = Geolocations.Geolocations.PerformMovement(currentLoc, destinationLoc, bg.group_speed);
                Location checkLoc = null;
                checkLoc = db.Location.Where(x => x.latitude == newLoc.latitude && x.longitude == newLoc.longitude).ToList().First();
                if(checkLoc == null)
                {
                    db.Location.Add(newLoc);
                    db.SaveChanges();

                    bg.current_loc_id = newLoc.id;
                }
                else
                {
                    bg.current_loc_id = checkLoc.id;
                }
            }

            return arrived;
        }

        private static void GetTraitModifiers(Users owner, MinionWarsEntitiesLib.Models.Battlegroup bg, int type)
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

        private static void SetBasicModifiers(MinionWarsEntitiesLib.Models.Battlegroup bg)
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

        private static void CalculateAdvancedModifiers(MinionWarsEntitiesLib.Models.Battlegroup bg, int count, int passive)
        {
            switch (passive)
            {
                case 0:
                    bg.str_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 1:
                    bg.dex_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 2:
                    bg.vit_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 3:
                    bg.pow_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 4:
                    bg.res_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 5:
                    bg.metal_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 6:
                    bg.stone_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 7:
                    bg.tree_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 8:
                    bg.food_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 9:
                    bg.build_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 10:
                    bg.movement_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 11:
                    bg.reproduction_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 12:
                    bg.loot_mod += GetModifierCoeficients(passive) * count;
                    break;
                case 13:
                    bg.regen_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(passive) * count));
                    break;
                case 14:
                    bg.resurrection_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(passive) * count));
                    break;
                case 15:
                    bg.defense_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(passive) * count));
                    break;
            }

            bg.group_speed = Convert.ToInt32(Math.Floor(1 + 1 * bg.movement_mod));
        }

        private static double GetModifierCoeficients(int id)
        {
            return db.ModifierCoeficients.Find(id).value;
        }

        /*private static void SetAdvancedModifiers(MinionWarsEntitiesLib.Models.Battlegroup bg, List<Minion> f, List<Minion> b, List<Minion> s)
        {
            bg.str_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.dex_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.vit_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.pow_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);

            bg.res_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.metal_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.stone_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.tree_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.food_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.build_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.movement_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.reproduction_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);
            bg.loot_mod += GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s);

            bg.regen_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s)));
            bg.resurrection_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s)));
            bg.defense_mod += Convert.ToInt32(Math.Floor(GetModifierCoeficients(0) * CountPassiveModifiers(0, f, b, s)));

            bg.group_speed = Convert.ToInt32(Math.Floor(1 + 1 * bg.movement_mod));
        }

        private static float CountPassiveModifiers(int passive, List<Minion> f, List<Minion> b, List<Minion> s)
        {
            return f.Where(x => x.passive == passive).Count() + b.Where(x => x.passive == passive).Count() + s.Where(x => x.passive == passive).Count();
        }

        private static List<Minion> SortMinions(List<int> m_id, int count, int type, int bg_id)
        {
            List<Minion> list = new List<Minion>();

            foreach (int id in m_id)
            {
                Minion minion = db.Minion.Find(id);
                //minion.battlegroup_id = bg_id;
                //minion.line = type;
            }

            return list;
        }*/
    }
}
