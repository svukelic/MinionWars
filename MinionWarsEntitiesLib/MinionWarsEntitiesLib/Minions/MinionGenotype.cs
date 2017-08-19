using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MinionWarsEntitiesLib.Minions
{
    public static class MinionGenotype
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static Minion generateRandomMinion()
        {
            Minion minion = new Minion();
            
            Random r = new Random();

            minion.mtype_id = r.Next(1, 16);
            minion.somatotype = SomatotypeGenerator(r);

            minion.strength = Convert.ToInt32(db.ModifierCoeficients.Find(18).value);
            minion.dexterity = Convert.ToInt32(db.ModifierCoeficients.Find(19).value);
            minion.vitality = Convert.ToInt32(db.ModifierCoeficients.Find(20).value);

            CalculateStatsByType(minion);
            CalculateStatsByBuild(minion);
            CalculateSpeed(minion);

            //minion.health = minion.vit * 3;

            minion.behaviour = r.Next(1, 16);
            minion.melee_ability = r.Next(0, 15) + 2; //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.ranged_ability = r.Next(0, 15) + 2; //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.passive = r.Next(0, 15);

            minion.speed = 1;

            List<Minion> checkMinion = null;
            checkMinion = db.Minion.Where(x => x.behaviour == minion.behaviour && x.melee_ability == minion.melee_ability && x.ranged_ability == minion.ranged_ability && x.passive == minion.passive && x.mtype_id == minion.mtype_id && x.somatotype.Equals(minion.somatotype)).ToList();
            if(checkMinion.Count > 0)
            {
                return checkMinion.First();
            }
            else
            {
                db.Minion.Add(minion);
                db.SaveChanges();
            }

            return minion;
        }

        private static void CalculateStatsByType(Minion m)
        {
            int strMod = 0;
            int dexMod = 0;
            int vitMod = 0;

            int powMod = 0;
            int cdMod = 0;
            int durMod = 0;

            //get from db
            MinionType mtype = db.MinionType.Find(m.mtype_id);
            strMod += mtype.str_modifier;
            dexMod += mtype.dex_modifier;
            vitMod += mtype.vit_modifier;

            powMod += mtype.pow_modifier;
            cdMod += mtype.cd_modifier;
            durMod += mtype.dur_modifier;

            m.strength += strMod;
            m.dexterity += dexMod;
            m.vitality += vitMod;

            m.power += powMod;
            m.cooldown += cdMod;
            m.duration += durMod;
        }

        private static void CalculateStatsByBuild(Minion m)
        {
            m.strength += int.Parse(m.somatotype[0].ToString()) - 4;
            m.dexterity += int.Parse(m.somatotype[1].ToString()) - 4;
            m.vitality += int.Parse(m.somatotype[2].ToString()) - 4;
        }

        private static void CalculateSpeed(Minion m)
        {
            m.speed = 1;
        }

        private static string SomatotypeGenerator(Random r)
        {
            int[] somatotypeValue = new int[3]; //Endomorph, mesomorph, ectomorph

            int first = r.Next(0, 2);
            somatotypeValue[first] = r.Next(1, 7);

            int second;
            if (first == 2) second = 0;
            else second = first + 1;
            do
            {
                somatotypeValue[second] = r.Next(1, 7);
            } while ((somatotypeValue[first] + somatotypeValue[second]) >= 12);

            int third;
            if (second == 2) third = 0;
            else third = second + 1;
            somatotypeValue[third] = 12 - (somatotypeValue[first] + somatotypeValue[second]);

            return string.Join("", somatotypeValue);
        }
    }
}