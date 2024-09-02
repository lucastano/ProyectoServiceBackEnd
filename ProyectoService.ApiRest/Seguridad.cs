using Microsoft.IdentityModel.Tokens;
using ProyectoService.LogicaNegocio.Modelo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProyectoService.ApiRest
{
    public static class Seguridad
    {

        public static void CrearPasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            //algoritmo de cifrado
            using (var hmac=new HMACSHA512())
            {
                passwordSalt = hmac.Key;//se usa para cifrar la contrasena
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));//computa el password en byte
            }
        }

        internal static bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac=new HMACSHA512(passwordSalt))
            {
                var passwordHashCalculado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return passwordHashCalculado.SequenceEqual(passwordHash);
                
            }
        }

        internal static string CrearToken(Usuario usuarioActual, IConfiguration configuration)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,usuarioActual.Email.Value),
                new Claim(ClaimTypes.Role,usuarioActual.Rol)
            };

            //generar clave secreta
            var claveSecreta = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:SecretTokenKey").Value!));
            //generar credenciales 
            var credenciales = new SigningCredentials(claveSecreta, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credenciales);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public static string GenerarPasswordRandom()
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyz";
            const string mayusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numeros = "0123456789";
            Random random = new Random();
            string resultadoCaracteres = string.Empty;

            string resultadoNumeros = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                int indice;
                if (i == 0)
                {
                    indice = random.Next(mayusculas.Length);
                    resultadoCaracteres+= mayusculas[indice];

                }
                else
                {
                     indice = random.Next(caracteres.Length);
                    resultadoCaracteres+= caracteres[indice];

                }
            }

            for (int j =0; j<7;j++)
            {
                int indice = random.Next(numeros.Length);
                resultadoNumeros+= numeros[indice];
            }



            return resultadoCaracteres+resultadoNumeros;

        }
    }
}
