﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class snEntities : DbContext
    {
        public snEntities()
            : base("name=snEntities")
        {
        }
    
        public snEntities(string connection_string)
            : base(connection_string)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cloud_srv> cloud_srv { get; set; }
        public virtual DbSet<d_msg> d_msg { get; set; }
        public virtual DbSet<dealer> dealer { get; set; }
        public virtual DbSet<event_calendar> event_calendar { get; set; }
        public virtual DbSet<istab> istab { get; set; }
        public virtual DbSet<ma> ma { get; set; }
        public virtual DbSet<mac_allowed> mac_allowed { get; set; }
        public virtual DbSet<note_calendar> note_calendar { get; set; }
        public virtual DbSet<problem> problem { get; set; }
        public virtual DbSet<serial> serial { get; set; }
        public virtual DbSet<serial_password> serial_password { get; set; }
        public virtual DbSet<spy_log> spy_log { get; set; }
        public virtual DbSet<training_calendar> training_calendar { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<websession> websession { get; set; }
    }
}
