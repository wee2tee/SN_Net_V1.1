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
    
    public partial class serial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public serial()
        {
            this.cloud_srv_Serial_id = new HashSet<cloud_srv>();
            this.ma_Serial_id = new HashSet<ma>();
            this.problem_Serial_id = new HashSet<problem>();
            this.serial_password_Serial_id = new HashSet<serial_password>();
        }
    
        public int id { get; set; }
        public string sernum { get; set; }
        public string oldnum { get; set; }
        public string version { get; set; }
        public string contact { get; set; }
        public string position { get; set; }
        public string prenam { get; set; }
        public string compnam { get; set; }
        public string addr01 { get; set; }
        public string addr02 { get; set; }
        public string addr03 { get; set; }
        public string zipcod { get; set; }
        public string telnum { get; set; }
        public string faxnum { get; set; }
        public string busityp { get; set; }
        public string busides { get; set; }
        public Nullable<System.DateTime> purdat { get; set; }
        public Nullable<System.DateTime> expdat { get; set; }
        public string howknown { get; set; }
        public string area { get; set; }
        public string branch { get; set; }
        public Nullable<System.DateTime> manual { get; set; }
        public string upfree { get; set; }
        public string refnum { get; set; }
        public string remark { get; set; }
        public string verext { get; set; }
        public Nullable<System.DateTime> verextdat { get; set; }
        public System.DateTime chgdat { get; set; }
        public int dealer_id { get; set; }
        public int recby { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cloud_srv> cloud_srv_Serial_id { get; set; }
        public virtual dealer dealer_id_Dealer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ma> ma_Serial_id { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<problem> problem_Serial_id { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serial_password> serial_password_Serial_id { get; set; }
        public virtual users recby_Users { get; set; }
    }
}