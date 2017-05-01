using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

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
            minion.build = r.Next(0, 15);
            minion.behavior = r.Next(0, 15);
            minion.melee = r.Next(0, 15);
            minion.ranged = r.Next(0, 15);
            minion.passive = r.Next(0, 15);

            minion.color = GenerateColor(minion);

            return minion;
        }


        private static Color GenerateColor(Minion minion)
        {
            Color color = new Color();

            int r = (int)(minion.type * 8) + (int)(minion.build * 8);
            int g = (int)(minion.behavior * 8) + (int)(minion.passive * 8);
            int b = (int)(minion.melee * 8) + (int)(minion.ranged * 8);

            color = Color.FromArgb(r, g, b);

            return color;
        }
    }
}