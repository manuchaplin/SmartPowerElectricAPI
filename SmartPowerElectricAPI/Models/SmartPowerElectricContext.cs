using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmartPowerElectricAPI.Models;

public partial class SmartPowerElectricContext : DbContext
{
    public SmartPowerElectricContext()
    {
    }

    public SmartPowerElectricContext(DbContextOptions<SmartPowerElectricContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<TipoMaterial> TipoMaterials { get; set; }

    public virtual DbSet<Trabajador> Trabajadors { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\SmartPowerElectric;Database=SmartPowerElectric;User Id=smartPower;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Eliminado).HasColumnName("eliminado");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEliminado)
                .HasColumnType("datetime")
                .HasColumnName("fechaEliminado");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Eliminado).HasColumnName("eliminado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEliminado)
                .HasColumnType("datetime")
                .HasColumnName("fechaEliminado");
            entity.Property(e => e.IdTipoMaterial).HasColumnName("idTipoMaterial");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("idUnidadMedida");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.TipoMaterial).WithMany(p => p.Materials)
                .HasForeignKey(d => d.IdTipoMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_TipoMaterial");

            entity.HasOne(d => d.UnidadMedidum).WithMany(p => p.Materials)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_UnidadMedida");
        });

        modelBuilder.Entity<TipoMaterial>(entity =>
        {
            entity.ToTable("TipoMaterial");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Eliminado).HasColumnName("eliminado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEliminado)
                .HasColumnType("datetime")
                .HasColumnName("fechaEliminado");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.ToTable("Trabajador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido).HasColumnName("apellido");
            entity.Property(e => e.CobroxHora).HasColumnName("cobroxHora");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Eliminado).HasColumnName("eliminado");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Especialidad).HasColumnName("especialidad");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEliminado)
                .HasColumnType("datetime")
                .HasColumnName("fechaEliminado");
            entity.Property(e => e.FechaFinContrato)
                .HasColumnType("datetime")
                .HasColumnName("fechaFinContrato");
            entity.Property(e => e.FechaInicioContrato)
                .HasColumnType("datetime")
                .HasColumnName("fechaInicioContrato");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.SeguridadSocial).HasColumnName("seguridadSocial");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<UnidadMedidum>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UMedida).HasColumnName("uMedida");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido).HasColumnName("apellido");
            entity.Property(e => e.Eliminado).HasColumnName("eliminado");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEliminado)
                .HasColumnType("datetime")
                .HasColumnName("fechaEliminado");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.Usuario1).HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
