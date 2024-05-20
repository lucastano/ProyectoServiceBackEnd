﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoService.AccesoDatos;

#nullable disable

namespace ProyectoService.AccesoDatos.Migrations
{
    [DbContext(typeof(ProyectoServiceContext))]
    partial class ProyectoServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("UsuarioSequence");

            modelBuilder.Entity("ProyectoService.LogicaNegocio.Modelo.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [UsuarioSequence]");

                    SqlServerPropertyBuilderExtensions.UseSequence(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("ProyectoService.LogicaNegocio.Modelo.Administrador", b =>
                {
                    b.HasBaseType("ProyectoService.LogicaNegocio.Modelo.Usuario");

                    b.ToTable("Administradores", (string)null);
                });

            modelBuilder.Entity("ProyectoService.LogicaNegocio.Modelo.Cliente", b =>
                {
                    b.HasBaseType("ProyectoService.LogicaNegocio.Modelo.Usuario");

                    b.Property<string>("Ci")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Clientes", (string)null);
                });

            modelBuilder.Entity("ProyectoService.LogicaNegocio.Modelo.Tecnico", b =>
                {
                    b.HasBaseType("ProyectoService.LogicaNegocio.Modelo.Usuario");

                    b.ToTable("Tecnicos", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
