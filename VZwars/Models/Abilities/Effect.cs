using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Abilities
{
    public abstract class Effect
    {
        public string name;
        public int activationTurn;
        public int duration;

        public abstract void PerformEffect();
    }
}