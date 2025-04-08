﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repositorio;

#nullable disable

namespace Repositorio.Migrations
{
    [DbContext(typeof(ContextoSql))]
    [Migration("20240531112837_AgregarPromociones")]
    partial class AgregarPromociones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dominio.Promocion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Etiqueta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PorcentajeDescuento")
                        .HasColumnType("int");

                    b.Property<int>("rangoFechasId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("rangoFechasId");

                    b.ToTable("Promociones");
                });

            modelBuilder.Entity("Dominio.RangoDeFechas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RangoDeFechas");
                });

            modelBuilder.Entity("Dominio.Usuario", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RolDeUsuario")
                        .HasColumnType("int");

                    b.Property<string>("VerificacionContrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Dominio.Promocion", b =>
                {
                    b.HasOne("Dominio.RangoDeFechas", "rangoFechas")
                        .WithMany()
                        .HasForeignKey("rangoFechasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("rangoFechas");
                });
#pragma warning restore 612, 618
        }
    }
}
