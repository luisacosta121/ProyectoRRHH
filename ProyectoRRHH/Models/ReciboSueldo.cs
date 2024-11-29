using System.ComponentModel.DataAnnotations;

namespace ProyectoRRHH.Models
{
    public class ReciboSueldo
    {

        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaCobro { get; set; }
        public double SueldoBruto { get; set; }

        public string UsuarioDni { get; set; }
        public Usuario Usuario { get; set; }


        public double SueldoNeto => SueldoBruto * (1 - Constantes.Descuento);
    }

    public static class Constantes
    {
        public const double Descuento = 0.17; // 17% de descuento
    }
}
