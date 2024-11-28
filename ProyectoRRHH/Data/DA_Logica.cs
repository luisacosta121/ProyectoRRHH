using Microsoft.AspNetCore.Authorization.Infrastructure;
using ProyectoRRHH.Models;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;

namespace ProyectoRRHH.Data
    {
    public class DA_Logica
    {
        private readonly EmpresaDatabaseContext _context;

        // Inyección de dependencias del DbContext
        public DA_Logica(EmpresaDatabaseContext context)
        {
            _context = context;
        }

        // Obtener la lista de usuarios desde la base de datos
        public List<Usuario> ListaUsuario()
        {
            return _context.Usuarios.ToList();  // Obtiene la lista de usuarios desde la tabla Usuarios en la base de datos
        }

        // Validar el usuario por correo y clave
        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return _context.Usuarios
                           .FirstOrDefault(u => u.Correo == _correo && u.Clave == _clave);
        }

    }
}