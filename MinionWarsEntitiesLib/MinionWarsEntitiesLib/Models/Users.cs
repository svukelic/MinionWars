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
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.Battlegroup = new HashSet<Battlegroup>();
            this.Camp = new HashSet<Camp>();
            this.Caravan = new HashSet<Caravan>();
            this.MinionOwnership = new HashSet<MinionOwnership>();
            this.Reputation = new HashSet<Reputation>();
            this.UserMovementHistory = new HashSet<UserMovementHistory>();
            this.UserTreasury = new HashSet<UserTreasury>();
        }
    
        public int id { get; set; }
        public string username { get; set; }
        public string pass { get; set; }
        public int experience { get; set; }
        public int lvl { get; set; }
        public System.Data.Entity.Spatial.DbGeography location { get; set; }
        public Nullable<int> online { get; set; }
        public int trait_leadership { get; set; }
        public int trait_logistics { get; set; }
        public int trait_architecture { get; set; }
        public int trait_economics { get; set; }
        public Nullable<int> personal_bg_id { get; set; }
        public Nullable<int> points { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Battlegroup> Battlegroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Camp> Camp { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Caravan> Caravan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MinionOwnership> MinionOwnership { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reputation> Reputation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserMovementHistory> UserMovementHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTreasury> UserTreasury { get; set; }
    }
}
