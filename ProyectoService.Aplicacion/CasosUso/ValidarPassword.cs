using ProyectoService.Aplicacion.ICasosUso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class ValidarPassword : IValidarPassword
    {
        public bool Ejecutar(string password)
        {
            //contrase;a debe tener por lo menos 1 mayuscula , 1 letra y 1 simbolo especial, por lo menos 4 numeros
            //NO PUEDE TENER ESPACIOS 
            
            bool valido = true;
            int letra = 0;
            int mayuscula = 0;
            int numeros = 0;  
            int contador = 0;
           

            while (valido && contador < password.Count())
            {
                if (char.IsSeparator(password[contador])|| char.IsPunctuation(password[contador]))
                {
                    valido = false;
                }
                if (char.IsNumber(password[contador]))
                {
                    numeros++;
                }
                if (char.IsLetter(password[contador]))
                {
                    letra++;
                    if (char.IsUpper(password[contador]))
                    {
                        mayuscula++;
                    }
                }
                
               
                contador++;
            }
            if (valido)
            {
                if (mayuscula>=1 && numeros>=4 && letra>=1 && contador>8 )
                {
                    valido = true;
                }
                else
                {
                    valido = false;
                }
            }

            return valido;
           
            
        }
    }
}
