using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProyectoRRHH.Models
{
    public class Usuario
    {
        [Key]
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }

        public string Roles { get; set; }

        public List<ReciboSueldo> ListaRecibos { get; set; }
        public List<Licencia> ListaLicencias { get; set; }

        [NotMapped] // No se agregará en la base de datos
        public string NombreCompleto => $"{Nombre} {Apellido}";

    }
}
