using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBancoAPI.Entities
{
    public class BitacoraEnt
    {


        public long IdBitacora { get; set; }
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }
        public string Origen { get; set; }
        public long IdUsuario { get; set; }

        public string DireccionIP { get; set; }

    }
}