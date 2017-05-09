﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using VZwars.Models.Abilities;
using VZwars.Models.Entities;

namespace VZwars.Models
{
    public class Minion
    {
        public int type { get; set; }
        public string build { get; set; }
        public int behavior { get; set; }
        public Ability melee { get; set; }
        public Ability ranged { get; set; }
        public int passive { get; set; }
        public Color color { get; set; }
        /*public string iconLink { get; set; }
        public Bitmap icon { get; set; }*/
        public Guid id { get; set; }
        public double speed { get; set; }
        public int line { get; set; }

        //public List<Ability> activeEffect { get; set; }

        public int str;
        public int dex;
        public int vit;
        public int health;
    }
}