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
        public static Minion generateRandomMinion()
        {
            using (var db = new MinionWarsEntities())
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
                minion.melee_ability = r.Next(1, 16) + 2; //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
                minion.ranged_ability = r.Next(1, 16) + 2; //AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
                minion.passive = r.Next(0, 15);

                minion.speed = 1;

                List<Minion> checkMinion = null;
                checkMinion = db.Minion.Where(x => x.behaviour == minion.behaviour && x.melee_ability == minion.melee_ability && x.ranged_ability == minion.ranged_ability && x.passive == minion.passive && x.mtype_id == minion.mtype_id && x.somatotype.Equals(minion.somatotype)).ToList();
                if (checkMinion.Count > 0)
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
        }

        public static Minion generateGeneticMinion(EvolutionPool ep1, EvolutionPool ep2)
        {
            using (var db = new MinionWarsEntities())
            {
                Minion minion = new Minion();

                Minion parent1 = db.Minion.Find(ep1.minion_id);
                Minion parent2 = db.Minion.Find(ep2.minion_id);

                Random r = new Random();

                int p1gene = -1;
                int p2gene = -1;

                do
                {
                    p1gene = r.Next(0, 5);
                    p2gene = r.Next(0, 5);
                } while (p1gene == p2gene);

                if (p1gene == 0) minion.mtype_id = parent1.mtype_id;
                else if(p2gene == 0) minion.mtype_id = parent2.mtype_id;
                else minion.mtype_id = r.Next(1, 16);

                if (p1gene == 1) minion.somatotype = parent1.somatotype;
                else if (p2gene == 1) minion.somatotype = parent2.somatotype;
                else minion.somatotype = SomatotypeGenerator(r);

                minion.strength = Convert.ToInt32(db.ModifierCoeficients.Find(18).value);
                minion.dexterity = Convert.ToInt32(db.ModifierCoeficients.Find(19).value);
                minion.vitality = Convert.ToInt32(db.ModifierCoeficients.Find(20).value);

                CalculateStatsByType(minion);
                CalculateStatsByBuild(minion);
                CalculateSpeed(minion);

                //TODO: BEHAVIOR GENE
                minion.behaviour = r.Next(1, 16);

                if (p1gene == 2) minion.melee_ability = parent1.melee_ability;
                else if (p2gene == 2) minion.melee_ability = parent2.melee_ability;
                else minion.melee_ability = r.Next(1, 16) + 2;

                if (p1gene == 3) minion.ranged_ability = parent1.ranged_ability;
                else if (p2gene == 3) minion.ranged_ability = parent2.ranged_ability;
                else minion.ranged_ability = r.Next(1, 16) + 2;

                if (p1gene == 4) minion.passive = parent1.passive;
                else if (p2gene == 4) minion.passive = parent2.passive;
                else minion.passive = r.Next(0, 15);

                minion.speed = 1;

                List<Minion> checkMinion = null;
                checkMinion = db.Minion.Where(x => x.behaviour == minion.behaviour && x.melee_ability == minion.melee_ability && x.ranged_ability == minion.ranged_ability && x.passive == minion.passive && x.mtype_id == minion.mtype_id && x.somatotype.Equals(minion.somatotype)).ToList();
                if (checkMinion.Count > 0)
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
        }

        private static void CalculateStatsByType(Minion m)
        {
            using (var db = new MinionWarsEntities())
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