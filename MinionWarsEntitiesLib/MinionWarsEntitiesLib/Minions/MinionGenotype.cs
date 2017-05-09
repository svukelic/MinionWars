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

            minion.mtype_id = r.Next(0, 15);
            minion.somatotype = SomatotypeGenerator(r);

            minion.strength = 6;
            minion.dexterity = 4;
            minion.vitality = 5;

            CalculateStatsByType(minion);
            CalculateStatsByBuild(minion);
            CalculateSpeed(minion);

            //minion.health = minion.vit * 3;

            minion.behaviour = r.Next(0, 15);
            minion.melee_ability = r.Next(0, 15); //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.ranged_ability = r.Next(0, 15); //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.passive = r.Next(0, 15);

            minion.speed = 1;

            Minion checkMinion = null;
            checkMinion = db.Minion.Where(x => x.behaviour == minion.behaviour && x.melee_ability == minion.melee_ability && x.ranged_ability == minion.ranged_ability && x.passive == minion.passive && x.mtype_id == minion.mtype_id && x.somatotype.Equals(minion.somatotype)).First();
            if(checkMinion != null)
            {
                return checkMinion;
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

            //get from db

            m.strength += strMod;
            m.dexterity += dexMod;
            m.vitality += vitMod;
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