using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Abilities
{
    public abstract class PassiveEffect
    {
        public string name;

        public abstract void AssignPassiveEffect();
    }
}