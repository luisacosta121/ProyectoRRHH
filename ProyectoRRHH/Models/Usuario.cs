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
        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre no puede exceder los 25 caracteres.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(25, ErrorMessage = "El apellido no puede exceder los 25 caracteres.")]
        public string Apellido { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime FechaIngreso { get; set; }


        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(15, ErrorMessage = "El teléfono no puede exceder los 15 caracteres.")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; }


        [Required(ErrorMessage = "La clave es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La clave debe tener al menos 3 caracteres.")]
        public string Clave { get; set; }

        public string Roles { get; set; }

        public List<ReciboSueldo> ListaRecibos { get; set; }
        public List<Licencia> ListaLicencias { get; set; }

        [NotMapped] // No se agregará en la base de datos
        public string NombreCompleto => $"{Nombre} {Apellido}";

    }
}
