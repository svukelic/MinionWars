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
    
    public partial class CostsMinion
    {
        public int id { get; set; }
        public int m_id { get; set; }
        public int r_id { get; set; }
        public Nullable<int> amount { get; set; }
    
        public virtual Minion Minion { get; set; }
        public virtual ResourceType ResourceType { get; set; }
    }
}