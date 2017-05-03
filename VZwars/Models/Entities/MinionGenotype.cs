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

            minion.type = new MinionType(r.Next(0, 15));
            minion.build = r.Next(0, 15);
            minion.behavior = r.Next(0, 15);
            minion.melee = AbilityGenerator.GenerateAbility(r.Next(0, 15), minion.type.numericValue, minion.build);
            minion.ranged = AbilityGenerator.GenerateAbility(r.Next(0, 15), minion.type.numericValue, minion.build);
            minion.passive = r.Next(0, 15);

            minion.speed = CalculateSpeed(minion.type.numericValue, minion.build);
            minion.stats = new MinionStats(minion.type.numericValue, minion.build);

            //minion.color = GenerateColor(minion);

            return minion;
        }


        /*private static Color GenerateColor(Minion minion)
        {
            Color color = new Color();

            int r = (int)(minion.type * 8) + (int)(minion.build * 8);
            int g = (int)(minion.behavior * 8) + (int)(minion.passive * 8);
            int b = (int)(minion.melee * 8) + (int)(minion.ranged * 8);

            color = Color.FromArgb(r, g, b);

            return color;
        }*/

        private static double CalculateSpeed(int type, int build)
        {
            return 1;
        }
    }
}