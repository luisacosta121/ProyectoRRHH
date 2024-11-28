namespace ProyectoRRHH.Models
{
    public class ReciboSueldo
    {

        public Guid Id { get; set; }
        public DateTime FechaCobro { get; set; }
        public double sueldo { get; set; }

        public string UsuarioDni { get; set; }
        public Usuario Usuario { get; set; }

    }
}
