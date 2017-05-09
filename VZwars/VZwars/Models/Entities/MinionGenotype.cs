using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using VZwars.Models.Abilities;
using VZwars.Models.Entities;

namespace VZwars.Models
{
    public static class MinionGenotype
    {
        public static Minion generateRandomMinion()
        {
            Minion minion = new Minion();

            minion.id = Guid.NewGuid();

            Random r = new Random();

            minion.type = r.Next(0, 15);
            minion.build = SomatotypeGenerator(r);

            minion.str = 6;
            minion.dex = 4;
            minion.vit = 5;

            CalculateStatsByType(minion);
            CalculateStatsByBuild(minion);
            CalculateSpeed(minion);

            minion.health = minion.vit * 3;

            minion.behavior = r.Next(0, 15);
            minion.melee = AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.ranged = AbilityGenerator.GenerateAbility(r.Next(0, 15), minion);
            minion.passive = r.Next(0, 15);

            return minion;
        }

        private static void CalculateStatsByType(Minion m)
        {
            switch (m.type)
            {
                case 0:
                    break;
            }
        }

        private static void CalculateStatsByBuild(Minion m)
        {
            m.str += int.Parse(m.build[0].ToString()) - 4;
            m.dex += int.Parse(m.build[1].ToString()) - 4;
            m.vit += int.Parse(m.build[2].ToString()) - 4;
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