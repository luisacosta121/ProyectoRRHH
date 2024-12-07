using System.ComponentModel.DataAnnotations;

namespace ProyectoRRHH.Models
{
    public class Licencia
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "La cantidad de días no puede ser negativo.")]
        public int CantDias { get; set; }

        public string UsuarioDni { get; set; }
        public Usuario Usuario { get; set; }
        public bool? Aprobado { get; set; }
    }
}
