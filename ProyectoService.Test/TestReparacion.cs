using Microsoft.EntityFrameworkCore;
using Moq;
using ProyectoService.AccesoDatos;
using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.LogicaNegocio.Excepciones;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Test
{
    [TestFixture]
    public class TestReparacion
    {
        private ProyectoServiceContext _context;
        private ReparacionEFRepositorio _reparacionRepositorio;
        private TecnicoEFRepositorio _tecnicoEFRepositorio;
        private ClienteEFRepositorio _clienteEFRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _reparacionRepositorio = new ReparacionEFRepositorio(_context);
            _tecnicoEFRepositorio = new TecnicoEFRepositorio(_context);
            _clienteEFRepositorio = new ClienteEFRepositorio(_context);
        }

        private void SeedData()
        {

            var salt = ObtenerSalt();

            var tecnico = new Tecnico
            {
                Nombre = "Juan",
                Apellido = "Lopez",
                Email = EmailVO.Crear("juan@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Tecnico"

            };
            string ciCliente = "48161123";
            var cliente = new Cliente
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Email = EmailVO.Crear("maria@example.com"),
                Ci= ciCliente,
                PasswordHash = ObtenerHash(ciCliente, salt),
                PasswordSalt = salt,
                Rol = "Cliente",
                Direccion="manzana 39 solar 17",
                Telefono="3472759"
                
            };

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "21312312",
                Descripcion = "no prende"

            };
            Reparacion reparacion2 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado="Presupuestada"

            };

            _context.Reparaciones.Add(reparacion);
            _context.SaveChanges();
            _context.Reparaciones.Add(reparacion2);
            _context.SaveChanges();

        }



        [Test]
        public async Task Add_ShouldAddReparacion()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                FechaPromesaPresupuesto=new DateTime(2024,07,10)


            };

            // Act
            Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);

            // Assert
           
            Assert.IsNotNull(reparacionRet);
        }


        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalTecnico()
        {
   
            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = null,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                FechaPromesaPresupuesto = new DateTime(2024, 07, 10)


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(  async  () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });
            string msg = ex.Message;
            Assert.AreEqual("Debe ingresar un tecnico", ex.Message);
            // Act




        }
        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalCliente()
        {

            //BUSCO EL CLIENTE POR CI 
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);


            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = null,
                Producto = "ps5",
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                FechaPromesaPresupuesto = new DateTime(2024, 07, 10)


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });
            
            Assert.AreEqual("Debe ingresar un cliente", ex.Message);
            // Act




        }

        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalNumeroSerie()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = null,
                Descripcion = "no prende",
                FechaPromesaPresupuesto = new DateTime(2024, 07, 10)


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });

            Assert.AreEqual("Debe ingresar numero de serie", ex.Message);
            // Act




        }
        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalDescripcion()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "2312312",
                Descripcion = null,
                FechaPromesaPresupuesto = new DateTime(2024, 07, 10)


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });

            Assert.AreEqual("Debe ingresar una descripcion", ex.Message);
            // Act




        }
        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenNullReparacion()
        {
            
            

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(null);
            });

            Assert.AreEqual("Debe ingresar una reparacion", ex.Message);
            // Act




        }

        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalProducto()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = null,
                NumeroSerie = "2312312",
                Descripcion = "no lee",
                FechaPromesaPresupuesto = new DateTime(2024, 07, 10)


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });

            Assert.AreEqual("Debe ingresar un producto", ex.Message);
            // Act




        }
        [Test]
        public async Task Add_ShouldThrowReparacionException_WhenInvalFechaPromesaPresupuesto()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = "ps5",
                NumeroSerie = "2312312",
                Descripcion = "no lee",
               


            };

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacionRet = await _reparacionRepositorio.AddAlternativo(reparacion);
            });

            Assert.AreEqual("Debe ingresar una fecha aproximada para el presupuesto", ex.Message);
            // Act




        }
        [Test]
        public async Task ShouldPresupuestar()
        {
           
            int id = 1;
            double ManoObra = 1000;
            string Descripcion = "no lee";
            DateTime fechaPromesaEntrega = new DateTime(2024, 06, 29);
            Reparacion reparacion = await _reparacionRepositorio.Presupuestar(id, ManoObra, Descripcion, fechaPromesaEntrega);
            Assert.IsNotNull(reparacion);
        }
        [Test]
        public async Task ShouldThrowReparacionExceptionPresupuestar_WhenIncorrectId()
        {
           
            int id = 100;
            double ManoObra = 1000;
            string Descripcion = "no lee";
            DateTime fechaPromesaEntrega = new DateTime(2024, 06, 29);

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacion = await _reparacionRepositorio.Presupuestar(id, ManoObra, Descripcion, fechaPromesaEntrega);
            });
            //Reparacion no existe
            Assert.AreEqual("Reparacion no existe", ex.Message);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionPresupuestar_WhenInvalidManoObra()
        {
            
            int id = 1;
            double ManoObra = 0;
            string Descripcion = "no lee";
            DateTime fechaPromesaEntrega = new DateTime(2024, 06, 29);

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacion = await _reparacionRepositorio.Presupuestar(id, ManoObra, Descripcion, fechaPromesaEntrega);
            });
            //Reparacion no existe
            Assert.AreEqual("Debe ingresar un valor de mano de obra", ex.Message);

        }
        [Test]
        public async Task ShouldThrowReparacionExceptionPresupuestar_WhenInvalidReparacionEstado()
        {
            //LE PASO EL ID DE UNA REPARACION QUE NO TIENE ESTADO EnTaller
            int id = 2;
            double ManoObra = 1000;
            string Descripcion = "no lee";
            DateTime fechaPromesaEntrega = new DateTime(2024, 06, 29);

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacion = await _reparacionRepositorio.Presupuestar(id, ManoObra, Descripcion, fechaPromesaEntrega);
            });
            //Reparacion no existe
            Assert.AreEqual("Esta reparacion ya fue presupuestada", ex.Message);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionPresupuestar_WhenInvalidDescripcion()
        {
            //SE LE PASA DESCRIPCION NULL
            int id = 1;
            double ManoObra = 1000;
            
            DateTime fechaPromesaEntrega = new DateTime(2024, 06, 29);

            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                Reparacion reparacion = await _reparacionRepositorio.Presupuestar(id, ManoObra, null, fechaPromesaEntrega);
            });
            //Reparacion no existe
            Assert.AreEqual("Debe ingresar una descripcion", ex.Message);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionAceptarPresupuesto_WhenInvalidEstadoReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION CON ESTADO EnTaller
            int id = 1;
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.AceptarPresupuesto(id);
            });

            Assert.AreEqual("Esta reparacion aun no esta presupuestada", ex.Message);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionAceptarPresupuesto_WhenInvalidReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION que no existe
            int id = 1000;
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.AceptarPresupuesto(id);
            });

            Assert.AreEqual("Reparacion no existe", ex.Message);

        }
        [Test]
        public async Task ShouldNullAceptarPresupuesto_WhenValidEstadoReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION QUE ESTA PRESUPUESTADA
            int id =2;
            Exception ex = null;
            try
            {
                await _reparacionRepositorio.AceptarPresupuesto(id);
            }
            catch(Exception e)
            {
                ex = e;
            }
           

            Assert.IsNull(ex);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionNOAceptarPresupuesto_WhenInvalidEstadoReparacion()
        {
            //int id, double costo,string razon
            int id = 1;
            double costo = 1000;
            string razon = "Muy caro";
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.NoAceptarPresupuesto(id,costo,razon);
            });

            Assert.AreEqual("Esta reparacion aun no esta presupuestada", ex.Message);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionNOAceptarPresupuesto_WhenInvalidReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION que no existe
            int id = 1000;
            double costo = 1000;
            string razon = "Muy caro";
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.NoAceptarPresupuesto(id,costo,razon);
            });

            Assert.AreEqual("Reparacion no existe", ex.Message);

        }
        [Test]
        public async Task ShouldNullNOAceptarPresupuesto_WhenValidEstadoReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION QUE ESTA PRESUPUESTADA
            int id = 2;
            double costo = 1000;
            string razon = "Muy caro";
            Exception ex = null;
            try
            {
                await _reparacionRepositorio.NoAceptarPresupuesto(id,costo,razon);
            }
            catch (Exception e)
            {
                ex = e;
            }


            Assert.IsNull(ex);

        }

        private byte[] ObtenerSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] ObtenerHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32);
            }
        }
    }
}

