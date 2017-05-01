using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class Minion
    {
        public int type { get; set; }
        public int build { get; set; }
        public int behavior { get; set; }
        public int melee { get; set; }
        public int ranged { get; set; }
        public int passive { get; set; }
        public Color color { get; set; }
        /*public string iconLink { get; set; }
        public Bitmap icon { get; set; }*/
        public Guid id { get; set; }
    }
}