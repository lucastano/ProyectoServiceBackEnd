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
        public DbSet<Empresa>Empresas { get; set; }



        public ProyectoServiceContext(DbContextOptions<ProyectoServiceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //value object configuration 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



            modelBuilder.Entity<Usuario>().UseTpcMappingStrategy();
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Tecnico>().ToTable("Tecnicos");
            modelBuilder.Entity<Administrador>().ToTable("Administradores");
        }

    }
}
