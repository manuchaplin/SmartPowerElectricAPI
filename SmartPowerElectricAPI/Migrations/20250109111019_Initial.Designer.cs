﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartPowerElectricAPI.Models;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    [DbContext(typeof(SmartPowerElectricContext))]
    [Migration("20250109111019_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SmartPowerElectricAPI.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("direccion");

                    b.Property<bool?>("Eliminado")
                        .HasColumnType("bit")
                        .HasColumnName("eliminado");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaCreacion");

                    b.Property<DateTime?>("FechaEliminado")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaEliminado");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nombre");

                    b.Property<int?>("Telefono")
                        .HasColumnType("int")
                        .HasColumnName("telefono");

                    b.HasKey("Id");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Cantidad")
                        .HasColumnType("float")
                        .HasColumnName("cantidad");

                    b.Property<bool?>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaEliminado")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdTipoMaterial")
                        .HasColumnType("int")
                        .HasColumnName("idTipoMaterial");

                    b.Property<int>("IdUnidadMedida")
                        .HasColumnType("int")
                        .HasColumnName("idUnidadMedida");

                    b.Property<double?>("Precio")
                        .HasColumnType("float")
                        .HasColumnName("precio");

                    b.HasKey("Id");

                    b.HasIndex("IdTipoMaterial");

                    b.HasIndex("IdUnidadMedida");

                    b.ToTable("Material", (string)null);
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.TipoMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Eliminado")
                        .HasColumnType("bit")
                        .HasColumnName("eliminado");

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaCreacion");

                    b.Property<DateTime?>("FechaEliminado")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaEliminado");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.ToTable("TipoMaterial", (string)null);
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.Trabajador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("apellido");

                    b.Property<double?>("CobroxHora")
                        .HasColumnType("float")
                        .HasColumnName("cobroxHora");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("direccion");

                    b.Property<bool?>("Eliminado")
                        .HasColumnType("bit")
                        .HasColumnName("eliminado");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Especialidad")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("especialidad");

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaCreacion");

                    b.Property<DateTime?>("FechaEliminado")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaEliminado");

                    b.Property<DateTime?>("FechaFinContrato")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaFinContrato");

                    b.Property<DateTime?>("FechaInicioContrato")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaInicioContrato");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nombre");

                    b.Property<string>("SeguridadSocial")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("seguridadSocial");

                    b.Property<int?>("Telefono")
                        .HasColumnType("int")
                        .HasColumnName("telefono");

                    b.HasKey("Id");

                    b.ToTable("Trabajador", (string)null);
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.UnidadMedidum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("UMedida")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("uMedida");

                    b.HasKey("Id");

                    b.ToTable("UnidadMedida");
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("apellido");

                    b.Property<bool?>("Eliminado")
                        .HasColumnType("bit")
                        .HasColumnName("eliminado");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaCreacion");

                    b.Property<DateTime?>("FechaEliminado")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaEliminado");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nombre");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<int?>("Telefono")
                        .HasColumnType("int")
                        .HasColumnName("telefono");

                    b.Property<string>("Usuario1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuario");

                    b.HasKey("Id");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.Material", b =>
                {
                    b.HasOne("SmartPowerElectricAPI.Models.TipoMaterial", "IdTipoMaterialNavigation")
                        .WithMany("Materials")
                        .HasForeignKey("IdTipoMaterial")
                        .IsRequired()
                        .HasConstraintName("FK_Material_TipoMaterial");

                    b.HasOne("SmartPowerElectricAPI.Models.UnidadMedidum", "IdUnidadMedidaNavigation")
                        .WithMany("Materials")
                        .HasForeignKey("IdUnidadMedida")
                        .IsRequired()
                        .HasConstraintName("FK_Material_UnidadMedida");

                    b.Navigation("IdTipoMaterialNavigation");

                    b.Navigation("IdUnidadMedidaNavigation");
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.TipoMaterial", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("SmartPowerElectricAPI.Models.UnidadMedidum", b =>
                {
                    b.Navigation("Materials");
                });
#pragma warning restore 612, 618
        }
    }
}