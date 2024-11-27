using Microsoft.AspNetCore.Authorization.Infrastructure;
using ProyectoRRHH.Models;

namespace ProyectoRRHH.Data
{
    public class DA_Logica
    {

      public List<Usuario> ListaUsuario()
        {
            return new List<Usuario>
            {
                new Usuario{ Nombre = "Administrador", Correo = "administrador@gmail.com", Clave = "123", Roles = new string[]{"Administrador" } },
                new Usuario{ Nombre = "Empleado1", Correo = "empleado1@gmail.com", Clave = "123", Roles = new string[]{"Empleado" } },
                new Usuario{ Nombre = "Empleado2", Correo = "empleado2@gmail.com", Clave = "123", Roles = new string[]{"Empleado" } },
                new Usuario{ Nombre = "Empleado3", Correo = "empleado3@gmail.com", Clave = "123", Roles = new string[]{"Empleado" } }


            };

        }

        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return ListaUsuario().Where(item => item.Correo == _correo && item.Clave == _clave).FirstOrDefault();
        }

    }
}
