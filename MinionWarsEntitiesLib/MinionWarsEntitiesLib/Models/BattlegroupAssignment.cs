//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MinionWarsEntitiesLib.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BattlegroupAssignment
    {
        public int id { get; set; }
        public int battlegroup_id { get; set; }
        public int minion_id { get; set; }
        public int group_count { get; set; }
        public int line { get; set; }
    
        public virtual Battlegroup Battlegroup { get; set; }
        public virtual Minion Minion { get; set; }
    }
}