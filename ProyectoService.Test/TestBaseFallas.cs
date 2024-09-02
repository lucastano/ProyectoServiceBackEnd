using Microsoft.EntityFrameworkCore;
using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.AccesoDatos;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Tls;
using ProyectoService.LogicaNegocio.Excepciones;
using ProyectoService.LogicaNegocio.IRepositorios;

namespace ProyectoService.Test
{
    [TestFixture]

    public class TestBaseFallas
    {
        private ProyectoServiceContext _context;
        private BaseFallaEFRepositorio _baseFallaRepositorio;
        private ProductoRepositorio _productoRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _baseFallaRepositorio = new BaseFallaEFRepositorio(_context);
            _productoRepositorio = new ProductoRepositorio(_context);
        }

        private void SeedData()
        {


            var producto1 = new Producto() 
            {
                Marca="Sony",
                Modelo="Ps5",
                Version="Slim"
            
            };
            var producto2 = new Producto()
            {
                Marca = "Microsoft",
                Modelo = "XBOX",
                Version = "360"

            };

            _context.Productos.AddRange(producto1, producto2);
            _context.SaveChanges();
        }



        [Test]
        public async Task Add_ShouldAddFalla()
        {
            //falla se agrega correctamente
            Producto producto1 = await _productoRepositorio.ObtenerProductoPorId(1);
            BaseFalla falla = new BaseFalla()
            {
                Producto = producto1,
                Falla="no lee",
                Solucion="Cambio de lector"

            };
            try
            {
                await _baseFallaRepositorio.Add(falla);

            }
            catch (Exception ex)
            {
                Assert.True(false, "Excepcion");
            }
            
          
        }
        [Test]
        public async Task Add_ShouldThrowException_whenEmptyFalla()
        {
            //falla se agrega correctamente
            Producto producto1 = await _productoRepositorio.ObtenerProductoPorId(1);
            BaseFalla falla = new BaseFalla()
            {
                Producto = producto1,
                Falla = "",
                Solucion = "Cambio de lector"

            };
            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                 await _baseFallaRepositorio.Add(falla);
            });

            Assert.AreEqual("Debe ingresar una falla", ex.Message);


        }

        [Test]
        public async Task Add_ShouldThrowException_whenEmptySolucion()
        {
            //falla se agrega correctamente
            Producto producto1 = await _productoRepositorio.ObtenerProductoPorId(1);
            BaseFalla falla = new BaseFalla()
            {
                Producto = producto1,
                Falla = "no lee",
                Solucion = ""

            };
            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _baseFallaRepositorio.Add(falla);
            });

            Assert.AreEqual("Debe ingresar una solucion", ex.Message);


        }

        [Test]
        public async Task Add_ShouldThrowException_whenFallaNull()
        {
            //se le pasa un producto null a la falla 

            BaseFalla falla = null;
            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _baseFallaRepositorio.Add(falla);
            });

            Assert.AreEqual("Debe ingresar una falla", ex.Message);


        }

        [Test]
        public async Task Add_ShouldThrowException_whenProductoNull()
        {
            //falla se agrega correctamente
            Producto producto1 = await _productoRepositorio.ObtenerProductoPorId(1);
            BaseFalla falla = new BaseFalla()
            {
                Producto = null,
                Falla = "no lee",
                Solucion = "cambio de lector"

            };
            var ex = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _baseFallaRepositorio.Add(falla);
            });

            Assert.AreEqual("Debe ingresar un producto", ex.Message);


        }



    }
}
