﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QConsole.DAL.EF.EDM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BaseEntities : DbContext
    {
        public BaseEntities(string conn)
            : base(string.Format("metadata=res://*/EF.EDM.QgisbaseModel.csdl|res://*/EF.EDM.QgisbaseModel.ssdl|res://*/EF.EDM.QgisbaseModel.msl;provider=Npgsql;provider connection string=\"{0}\"", conn.Replace('"', ' '))) //"Host=127.0.0.1;Database=MY_BASE;Username=admin;Password=1"
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<logtable> logtable { get; set; }
        public virtual DbSet<dictionaries> dictionaries { get; set; }
    }
}
