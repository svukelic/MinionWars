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
    
    public partial class OffensiveBuilding
    {
        public int id { get; set; }
        public string name { get; set; }
        public int camp_id { get; set; }
        public Nullable<int> minion_id { get; set; }
    
        public virtual Camp Camp { get; set; }
        public virtual Minion Minion { get; set; }
    }
}