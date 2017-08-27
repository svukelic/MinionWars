﻿using MinionWarsEntitiesLib.Battlegroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinionWarsEntitiesLib.Abilities
{
    public abstract class Effect
    {
        public abstract void PerformEffect(BattleGroupEntity targetGroup, Ability abilityData, int count);
    }
}