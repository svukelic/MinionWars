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
    
    public partial class Battlegroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Battlegroup()
        {
            this.BattlegroupAssignment = new HashSet<BattlegroupAssignment>();
            this.BattlegroupMovementHistory = new HashSet<BattlegroupMovementHistory>();
        }
    
        public int id { get; set; }
        public Nullable<int> owner_id { get; set; }
        public int size { get; set; }
        public Nullable<int> orders_id { get; set; }
        public int group_speed { get; set; }
        public double str_mod { get; set; }
        public double dex_mod { get; set; }
        public double vit_mod { get; set; }
        public double pow_mod { get; set; }
        public double res_mod { get; set; }
        public double metal_mod { get; set; }
        public double stone_mod { get; set; }
        public double tree_mod { get; set; }
        public double food_mod { get; set; }
        public double build_mod { get; set; }
        public double movement_mod { get; set; }
        public double reproduction_mod { get; set; }
        public double loot_mod { get; set; }
        public int regen_mod { get; set; }
        public int resurrection_mod { get; set; }
        public int defense_mod { get; set; }
        public System.Data.Entity.Spatial.DbGeography location { get; set; }
        public Nullable<System.DateTime> lastMovement { get; set; }
        public int type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BattlegroupAssignment> BattlegroupAssignment { get; set; }
        public virtual Orders Orders { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BattlegroupMovementHistory> BattlegroupMovementHistory { get; set; }
    }
}
