﻿using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface IClienteRepositorio:ICrudRepositorio<Cliente>
    {
       
        Cliente GetClienteByCi(string ci);


    }
}
