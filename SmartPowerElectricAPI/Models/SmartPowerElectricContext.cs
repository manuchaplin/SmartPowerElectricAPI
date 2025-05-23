﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmartPowerElectricAPI.Models;

public class SmartPowerElectricContext : DbContext
{
    public SmartPowerElectricContext()
    {
    }

    public SmartPowerElectricContext(DbContextOptions<SmartPowerElectricContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }
    public virtual DbSet<DocumentoCaducar> DocumentoCaducar { get; set; }
    public virtual DbSet<Factura> Factura { get; set; }
    public virtual DbSet<Material> Material { get; set; }
    public virtual DbSet<Nomina> Nomina { get; set; }
    public virtual DbSet<Orden> Orden { get; set; }
    public virtual DbSet<Proyecto> Proyecto { get; set; }
    public virtual DbSet<TipoMaterial> TipoMaterial { get; set; }
    public virtual DbSet<Trabajador> Trabajador { get; set; }
    public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }
   



    //local
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(localdb)\\SmartPowerElectric;Database=SmartPowerElectric;User Id=smartPower;Password=12345;TrustServerCertificate=True;");

    //Pro
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
      => optionsBuilder.UseSqlServer("Server=smartpowerelectric.database.windows.net;Database=SmartPowerElectric;User Id=smartPower;Password=Sp.*95072728906;TrustServerCertificate=True;");

}
