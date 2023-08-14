using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBancoAPI.Entities
{
    public class RoleEnt
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; }

        public string Estado { get; set; }

        public int IdUsuario { get; set;}
    }
}