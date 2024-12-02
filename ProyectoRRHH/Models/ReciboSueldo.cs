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

        public bool Firmado { get; set; }


        public double SueldoNeto => SueldoBruto * (1 - Constantes.Descuento);
        public double Jubilacion => SueldoBruto * (Constantes.DescuentoJubilacion);
        public double ObraSocial => SueldoBruto * (Constantes.DescuentoObraSocial);
        public double Ley19032 => SueldoBruto * (Constantes.DescuentoLey);
    }

    public static class Constantes
    {
        public const double Descuento = 0.17; // Descuento total

        public const double DescuentoJubilacion = 0.11; // 11% descuento jubilacion

        public const double DescuentoObraSocial = 0.03; // 3% descuento obra social

        public const double DescuentoLey = 0.03; // 3% descuento ley 19032
    }
}
