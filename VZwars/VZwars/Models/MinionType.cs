//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VZwars.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MinionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MinionType()
        {
            this.Minion = new HashSet<Minion>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int str_modifier { get; set; }
        public int dex_modifier { get; set; }
        public int vit_modifier { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Minion> Minion { get; set; }
    }
}
