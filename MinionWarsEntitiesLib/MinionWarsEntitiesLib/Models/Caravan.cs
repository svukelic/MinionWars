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
    
    public partial class Caravan
    {
        public int id { get; set; }
        public Nullable<int> owner_id { get; set; }
        public Nullable<int> source_id { get; set; }
        public Nullable<int> destination_id { get; set; }
        public System.Data.Entity.Spatial.DbGeography location { get; set; }
        public string directions { get; set; }
        public System.Data.Entity.Spatial.DbGeography current_step { get; set; }
        public Nullable<System.DateTime> last_movement { get; set; }
    
        public virtual Camp Camp { get; set; }
        public virtual Camp Camp1 { get; set; }
        public virtual Users Users { get; set; }
    }
}
