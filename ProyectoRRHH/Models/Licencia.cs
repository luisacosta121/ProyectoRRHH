using System.ComponentModel.DataAnnotations;

namespace ProyectoRRHH.Models
{
    public class Licencia
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        public int CantDias { get; set; }

        public string UsuarioDni { get; set; }
        public Usuario Usuario { get; set; }
        public bool? Aprobado { get; set; }
    }
}
