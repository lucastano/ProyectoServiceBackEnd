using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos
{
    public class ProyectoServiceContext:DbContext
    {
        //public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente>Clientes { get; set; }
        public DbSet<Tecnico>Tecnicos { get; set; }
        public DbSet<Administrador>Administradores { get; set; }
        public DbSet<Reparacion>Reparaciones { get; set; }
        public DbSet<Mensaje> Mensajes {  get; set; }   
       
        public DbSet<BaseFalla>BaseFallas { get; set; }
        public DbSet<Producto>Productos { get; set; }


        public ProyectoServiceContext(DbContextOptions<ProyectoServiceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //value object configuration 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //CONFIGURACION DE HERENCIA
            modelBuilder.Entity<Usuario>().UseTpcMappingStrategy();
            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Tecnico>().ToTable("tecnicos");
            modelBuilder.Entity<Administrador>().ToTable("administradores");


            //CONFIGURACION DE MENSAJERIA
            modelBuilder.Entity<Reparacion>()
                .HasMany(r => r.Mensajes)
                .WithOne(m => m.Reparacion)
                .HasForeignKey(m => m.ReparacionId);

            modelBuilder.Entity<Mensaje>()
                .HasOne(m=>m.Emisor)
                .WithMany()
                .HasForeignKey(m=>m.EmisorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mensaje>()
                .HasOne(m=>m.Destinatario)
                .WithMany()
                .HasForeignKey(m=>m.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }

    }
}
