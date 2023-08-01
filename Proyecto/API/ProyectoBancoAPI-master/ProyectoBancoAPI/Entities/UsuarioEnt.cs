namespace ProyectoBancoAPI.Entities
{
    public class UsuarioEnt
    {
        public long IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo{ get; set; }

        public string Contrasenna { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public int IdRole { get; set; }

        public string NombreRole { get; set; }

        public string ContrasennaNueva { get; set; }

        public string Identificacion { get; set; }

        public bool Estado { get; set; }
    }
}