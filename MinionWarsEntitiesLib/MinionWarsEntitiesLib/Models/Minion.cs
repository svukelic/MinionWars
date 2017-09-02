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
    
    public partial class Minion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Minion()
        {
            this.BattlegroupAssignment = new HashSet<BattlegroupAssignment>();
            this.CostsMinion = new HashSet<CostsMinion>();
            this.HiveNode = new HashSet<HiveNode>();
            this.OffensiveBuilding = new HashSet<OffensiveBuilding>();
            this.MinionOwnership = new HashSet<MinionOwnership>();
        }
    
        public int id { get; set; }
        public int mtype_id { get; set; }
        public string somatotype { get; set; }
        public int melee_ability { get; set; }
        public int ranged_ability { get; set; }
        public int passive { get; set; }
        public double speed { get; set; }
        public int strength { get; set; }
        public int dexterity { get; set; }
        public int vitality { get; set; }
        public int behaviour { get; set; }
        public int power { get; set; }
        public int cooldown { get; set; }
        public int duration { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BattlegroupAssignment> BattlegroupAssignment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CostsMinion> CostsMinion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HiveNode> HiveNode { get; set; }
        public virtual MinionType MinionType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OffensiveBuilding> OffensiveBuilding { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MinionOwnership> MinionOwnership { get; set; }
    }
}
