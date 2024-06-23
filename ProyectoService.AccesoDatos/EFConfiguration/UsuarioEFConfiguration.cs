using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.EFConfiguration
{
    public class UsuarioEFConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(e => e.Email)
               .HasConversion(
               v => v.Value,
               v => EmailVO.Crear(v));
        }
    }
}
