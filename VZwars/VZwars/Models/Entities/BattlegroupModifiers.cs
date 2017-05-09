using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Entities
{
    public class BattlegroupModifiers
    {
        //Combat modifiers
        public double strMod = 0;
        public double dexMod = 0;
        public double vitMod = 0;
        public double powMod = 0;
        public double cdMod = 0;
        public int regenMod = 0;
        public int resurrectionMod = 0;

        //Map modifiers
        public double resMod = 0;
        public double lootMod = 0;
        public double stoneMod = 0;
        public double treeMod = 0;
        public double buildMod = 0;
        public double metalMod = 0;
        public double reproductionMod = 0;
        public double movementMod = 0;
        public double foodMod = 0;

        public double defenseMod = 0;
    }
}