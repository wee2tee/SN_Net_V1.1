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
    
    public partial class cloud_srv
    {
        public int id { get; set; }
        public System.DateTime start_date { get; set; }
        public System.DateTime end_date { get; set; }
        public string email { get; set; }
        public System.DateTime chgdat { get; set; }
        public int serial_id { get; set; }
        public int recby { get; set; }
    
        public virtual users users { get; set; }
        public virtual serial serial { get; set; }
    }
}
