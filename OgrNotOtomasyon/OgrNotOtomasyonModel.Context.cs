﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OgrNotOtomasyon
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OgrNotOtomasyonEntities : DbContext
    {
        public OgrNotOtomasyonEntities()
            : base("name=OgrNotOtomasyonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<M_DERSLER> M_DERSLER { get; set; }
        public virtual DbSet<M_KULUPLER> M_KULUPLER { get; set; }
        public virtual DbSet<M_NOTLAR> M_NOTLAR { get; set; }
        public virtual DbSet<M_OGRENCI> M_OGRENCI { get; set; }
    
        public virtual ObjectResult<NOTLISTESI_Result> NOTLISTESI()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NOTLISTESI_Result>("NOTLISTESI");
        }
    }
}
