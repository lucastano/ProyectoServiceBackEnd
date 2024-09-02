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
using System.Collections;
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
        private ProductoRepositorio _productoRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            
            _reparacionRepositorio = new ReparacionEFRepositorio(_context);
            _tecnicoEFRepositorio = new TecnicoEFRepositorio(_context);
            _clienteEFRepositorio = new ClienteEFRepositorio(_context);
            _productoRepositorio = new ProductoRepositorio(_context);
            SeedData();
        }

        private async void SeedData()
        {

            var salt = ObtenerSalt();
            var producto1 = new Producto()
            {
                Marca = "Sony",
                Modelo = "PS5",
                Version = "slim"

            };
            var producto2 = new Producto()
            {
                Marca = "Microsoft",
                Modelo = "Xbox",
                Version = "x"

            };
            
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
                Producto = producto1,
                NumeroSerie = "21312312",
                Descripcion = "no prende"

            };
            Reparacion reparacion2 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado="Presupuestada"

            };
            Reparacion reparacion3 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto1,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado = "Aceptada"

            };
            Reparacion reparacion4 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado = "Terminada"

            };
            _context.Productos.Add(producto1);
            _context.SaveChanges();
            _context.Productos.Add(producto2);
            _context.SaveChanges();
            _context.Reparaciones.Add(reparacion);
            _context.SaveChanges();
            _context.Reparaciones.Add(reparacion2);
            _context.SaveChanges();
            _context.Reparaciones.Add(reparacion3);
            _context.SaveChanges();
            _context.Reparaciones.Add(reparacion4);
            _context.SaveChanges();
            

            Reparacion reparacion5 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado = "EnTaller"

            };
            _context.Reparaciones.Add(reparacion5);
            _context.SaveChanges();
            //presupuesto la reparacion 5
            await _reparacionRepositorio.Presupuestar(5, 1000, "prueba", new DateTime(2024,08,24));

            //REPARACION EN TALLER PARA PRUEBAS PRESUPUESTAR QUE FALLEN
            Reparacion reparacion6 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado = "EnTaller"

            };
            _context.Reparaciones.Add(reparacion6);
            _context.SaveChanges();





        }



        [Test]
        public async Task Add_ShouldAddReparacion()
        {
            // Arrange
            //BUSCO EL TECNICO POR EMAIL
            string email = "juan@example.com";
            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);
            
            Producto producto1 = await _productoRepositorio.ObtenerProductoPorId(1);
            

            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto1,
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
            Producto producto2 = await _productoRepositorio.ObtenerProductoPorId(2);
            Reparacion reparacion = new Reparacion()
            {
                Tecnico = null,
                Cliente = cliente,
                Producto = producto2,
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
            Producto producto2 = await _productoRepositorio.ObtenerProductoPorId(2);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = null,
                Producto = producto2,
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
            Producto producto2 = await _productoRepositorio.ObtenerProductoPorId(2);
            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
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
            Tecnico? tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorEmail(email);
            Producto producto2 = await _productoRepositorio.ObtenerProductoPorId(2);
            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente? cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
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
            Producto producto2 = await _productoRepositorio.ObtenerProductoPorId(2);
            //BUSCO EL CLIENTE POR CI 
            string ciCliente = "48161123";
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi(ciCliente);

            Reparacion reparacion = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto2,
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
            int id =5;
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
            int id = 5;
            Reparacion rep = await _reparacionRepositorio.ObtenerReparacionPorId(id);

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
        [Test]
        public async Task ShouldThrowReparacionExceptionTerminarReparacion_WhenInvalidEstadoReparacion()
        {
            //ACEPTA REPARACIONES CON ESTADO NOACEPTADAS, ACEPTADAS
            //SE LE PASA UNA REPARACION CON ESTADO ENTALLER
            int id = 1;
            bool reparada = true;

            // Definir los mensajes de error esperados
            List<string> expectedMessages = new List<string>
    {
        "Esta reparacion ya fue terminada",
        "Esta reparacion aun no se puede terminar",
        "Esta reparacion ya fue entregada"
        // Agrega más mensajes de error esperados aquí según sea necesario
    };

            // Verificar la excepción y el mensaje esperado
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.Terminar(id, reparada);
            });

            // Verificar que el mensaje de la excepción está entre los mensajes esperados
            Assert.Contains(ex.Message, expectedMessages);





        }

        [Test]
        public async Task ShouldThrowReparacionExceptionTerminarReparacion_WhenInvalidReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION que no existe
            int id = 1000;
            bool reparada = true;
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.Terminar(id,reparada);
            });

            Assert.AreEqual("Reparacion no existe", ex.Message);

        }

        [Test]
        public async Task ShouldTerminarReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION CON UN ESTADO CORRECTO PARA LA FASE DE TERMINAR, RETORNA EL OBJETO
            int id = 3;
            bool reparada = true;
            
              Reparacion reparacion=  await _reparacionRepositorio.Terminar(id, reparada);


            Assert.IsNotNull(reparacion);

        }

        [Test]
        public async Task ShouldEntregarReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION CON UN ESTADO CORRECTO PARA LA FASE DE ENTREGAR, RETORNA EL OBJETO
            int id = 4;
            

            Reparacion reparacion = await _reparacionRepositorio.Entregar(id);


            Assert.IsNotNull(reparacion);

        }

        [Test]
        public async Task ShouldThrowReparacionExceptionEntregarReparacion_WhenInvalidReparacion()
        {
            //SE LE PASE ID DE UNA REPARACION que no existe
            int id = 1000;
            bool reparada = true;
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.Entregar(id);
            });

            Assert.AreEqual("reparacion no existe", ex.Message);

        }
        [Test]
        public async Task ShouldThrowReparacionExceptionEntregarReparacion_WhenInvalidEstado()
        {
            //SE LE PASE ID DE UNA REPARACION que no existe
            int id = 1;
            bool reparada = true;
            var ex = Assert.ThrowsAsync<ReparacionException>(async () =>
            {
                await _reparacionRepositorio.Entregar(id);
            });

            Assert.AreEqual("Esta reparacion aun no esta terminada", ex.Message);

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

