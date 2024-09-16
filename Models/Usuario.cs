namespace FARMACIA_JOSHUA_RESTFUL.Models{
    public class Usuario{
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public int IdRol { get; set; }
    }
}