//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SN_Net.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class dealer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dealer()
        {
            this.d_msg = new HashSet<d_msg>();
            this.serial = new HashSet<serial>();
        }
    
        public int id { get; set; }
        public string dealercode { get; set; }
        public string prenam { get; set; }
        public string compnam { get; set; }
        public string addr01 { get; set; }
        public string addr02 { get; set; }
        public string addr03 { get; set; }
        public string zipcod { get; set; }
        public string telnum { get; set; }
        public string faxnum { get; set; }
        public string contact { get; set; }
        public string position { get; set; }
        public string busides { get; set; }
        public string area { get; set; }
        public string remark { get; set; }
        public System.DateTime chgdat { get; set; }
        public int recby { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<d_msg> d_msg { get; set; }
        public virtual users users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serial> serial { get; set; }
    }
}
