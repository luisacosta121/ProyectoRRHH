﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoRRHH.Context;

#nullable disable

namespace ProyectoRRHH.Migrations
{
    [DbContext(typeof(EmpresaDatabaseContext))]
    partial class EmpresaDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProyectoRRHH.Models.ReciboSueldo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FechaCobro")
                        .HasColumnType("datetime2");

                    b.Property<double>("SueldoBruto")
                        .HasColumnType("float");

                    b.Property<string>("UsuarioDni")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioDni");

                    b.ToTable("ReciboSueldos");
                });

            modelBuilder.Entity("ProyectoRRHH.Models.Usuario", b =>
                {
                    b.Property<string>("Dni")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clave")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Dni");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ProyectoRRHH.Models.ReciboSueldo", b =>
                {
                    b.HasOne("ProyectoRRHH.Models.Usuario", "Usuario")
                        .WithMany("ListaRecibos")
                        .HasForeignKey("UsuarioDni");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ProyectoRRHH.Models.Usuario", b =>
                {
                    b.Navigation("ListaRecibos");
                });
#pragma warning restore 612, 618
        }
    }
}
