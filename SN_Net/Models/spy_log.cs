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
    
    public partial class spy_log
    {
        public int id { get; set; }
        public string serial_sernum { get; set; }
        public string compnam { get; set; }
        public System.DateTime chgdat { get; set; }
        public int users_id { get; set; }
    
        public virtual users users_id_Users { get; set; }
    }
}