using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoRRHH.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }

        public string Roles { get; set; }


    }
}
