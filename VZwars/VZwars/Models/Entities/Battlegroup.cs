using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VZwars.Models.Entities;

namespace VZwars.Models
{
    public class Battlegroup
    {
        public List<Minion> frontline;
        public List<Minion> backline;
        public List<Minion> support;

        public double groupSpeed = 1;

        BattlegroupModifiers modifiers;

        public Battlegroup()
        {

        }

        public Battlegroup(List<Minion> f, List<Minion> b, List<Minion> s)
        {
            this.frontline = f;
            this.backline = b;
            this.support = s;

            this.groupSpeed = CalculateGroupSpeed();
            this.modifiers = CalculateModifiers();
        }

        public double CalculateGroupSpeed()
        {
            /*double sum = 0;
            sum += this.frontline.Average(x => x.speed) + this.backline.Average(x => x.speed) + this.support.Average(x => x.speed);
            return Math.Floor(sum / 3);*/
            return this.groupSpeed + this.groupSpeed * this.modifiers.movementMod;
        }

        public BattlegroupModifiers CalculateModifiers()
        {
            BattlegroupModifiers mods = new BattlegroupModifiers();

            mods.strMod = 0.2 * SetMods(0);
            mods.dexMod = 0.2 * SetMods(1);
            mods.vitMod = 0.2 * SetMods(2);
            mods.powMod = 0.4 * SetMods(3);
            mods.cdMod = 0.4 * SetMods(4);

            mods.resMod = 0.50 * SetMods(5);
            mods.lootMod = 2 * SetMods(6);
            mods.stoneMod = 2 * SetMods(7);
            mods.treeMod = 2 * SetMods(8);
            mods.buildMod = 2 * SetMods(9);
            mods.metalMod = 2 * SetMods(10);
            mods.reproductionMod = 2 * SetMods(11);
            mods.movementMod = 0.6 * SetMods(12);
            mods.foodMod = 1 * SetMods(13);

            mods.regenMod = Convert.ToInt32(3 * SetMods(14));
            mods.resurrectionMod = Convert.ToInt32(1 * SetMods(15));

            return mods;
        }

        public double SetMods(int passive)
        {
            return this.frontline.Where(x => x.passive == 0).Count() + this.backline.Where(x => x.passive == 0).Count() + this.support.Where(x => x.passive == 0).Count();
        }

    }
}