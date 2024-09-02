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
using ProyectoService.LogicaNegocio.Servicios;
using ProyectoService.AccesoDatos.Servicios;
using System.Security.Cryptography;
using ProyectoService.LogicaNegocio.IRepositorios;

namespace ProyectoService.Test
{

    [TestFixture]
    public class TestMensajes
    {
        private ProyectoServiceContext _context;
        private MensajeriaEFRepositorio _mensajesRepositorio;
        private UsuarioServicio _usuarioService;
        private ReparacionServicio _reparacionService;
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
            _clienteEFRepositorio = new ClienteEFRepositorio(_context);
            _tecnicoEFRepositorio = new TecnicoEFRepositorio(_context);
            _productoRepositorio = new ProductoRepositorio(_context);
            SeedData();
            _mensajesRepositorio = new MensajeriaEFRepositorio(_context, _usuarioService,_reparacionService);

        }

        private async void SeedData()
        {
            var salt = ObtenerSalt();
            //cliente add
            var cliente1 = new Cliente
            {
                Nombre = "simon",
                Apellido = "carreno",
                Email = EmailVO.Crear("simon@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Cliente",
                Direccion = "Calle Principal",
                Telefono = "123456789",
                Ci = "12345678"
            };
           await _clienteEFRepositorio.Add(cliente1);
           _context.SaveChanges();

            //tecnico add
           
            var tecnico1 = new Tecnico
            {
                Nombre = "Juan",
                Apellido = "Lopez",
                Email = EmailVO.Crear("juan@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Tecnico"

            };
            await _tecnicoEFRepositorio.Add(tecnico1);
            _context.SaveChanges();

            Producto producto = new Producto()
            {
                Marca="Sony",
                Modelo="PS5",
                Version="Slim"

            };
            await _productoRepositorio.Add(producto);
            _context.SaveChanges();
            //obtener tecnico y obtener cliente

            Tecnico tecnico = await _tecnicoEFRepositorio.ObtenerTecnicoPorId(1);
            Cliente cliente = await _clienteEFRepositorio.GetClienteByCi("12345678");
            //add de reparacion EnTaller
            Reparacion reparacion5 = new Reparacion()
            {
                Tecnico = tecnico,
                Cliente = cliente,
                Producto = producto,
                NumeroSerie = "21312312",
                Descripcion = "no prende",
                Estado = "EnTaller"

            };
            _context.Reparaciones.Add(reparacion5);
            _context.SaveChanges();


            var mensaje = new Mensaje
            {
               ReparacionId=1,
               DestinatarioId=1,
               EmisorId=2,
               Texto="Hola",
               FechaHoraEnvio=DateTime.Now,

            };
            _context.Mensajes.Add(mensaje);
            _context.SaveChanges();
        }



        [Test]
        public async Task Add_ShouldNewMensaje()
        {


            //     public int Id { get; set; }
            //public int ReparacionId { get; set; }

            //public Usuario Emisor { get; set; }

            //public int EmisorId { get; set; }
            //public Usuario Destinatario { get; set; }
            //public int DestinatarioId { get; set; }

            //public DateTime FechaHoraEnvio { get; set; }
            //public Reparacion Reparacion { get; set; }
            //public string Texto { get; set; }
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
