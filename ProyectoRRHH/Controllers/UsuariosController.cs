using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;
using ProyectoRRHH.Models;

namespace ProyectoRRHH.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EmpresaDatabaseContext _context;

        public UsuariosController(EmpresaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string buscar, string filtro)
        {
            var usuarios = from usuario in _context.Usuarios select usuario;

            if (!String.IsNullOrEmpty(buscar))
            {
                usuarios = usuarios.Where(s => s.Apellido!.Contains(buscar) ||
                                        s.Dni.ToString().Contains(buscar));
            }

            ViewData["FiltroApellido"] = String.IsNullOrEmpty(filtro) ? "NombreDescendente" : "";
            ViewData["FiltroFechaNac"] = filtro == "FechaNacAscendente" ? "FechaNacDescendente" : "FechaNacAscendente";
            ViewData["FiltroFechaIng"] = filtro == "FechaIngAscendente" ? "FechaIngDescendente" : "FechaIngAscendente";
            ViewData["FiltroDni"] = filtro == "DniAscendente" ? "DniDescendente" : "DniAscendente";

            switch (filtro)
            {
                case "NombreDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.Apellido);
                    break;
                case "FechaNacDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.FechaNacimiento);
                    break;
                case "FechaNacAscendente":
                    usuarios = usuarios.OrderBy(usuario => usuario.FechaNacimiento);
                    break;
                case "FechaIngDescendente":
                    usuarios = usuarios.OrderByDescending(usuario => usuario.FechaIngreso);
                    break;
                case "FechaIngAscendente":
                    usuarios = usuarios.OrderBy(usuario => usuario.FechaIngreso);
                    break;
                case "DniDescendente":
                    usuarios = usuarios.OrderByDescending(r => r.Dni);
                    break;
                case "DniAscendente":
                    usuarios = usuarios.OrderBy(r => r.Dni);
                    break;
                default:
                    usuarios = usuarios.OrderBy(usuario => usuario.Apellido);
                    break;
            }

            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Nombre == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dni,Nombre,Apellido, FechaNacimiento, FechaIngreso, Telefono, Correo,Clave,Roles")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Dni,Nombre,Apellido,FechaNacimiento,FechaIngreso,Telefono,Correo,Clave,Roles")] Usuario usuario)
        {
            if (id != usuario.Dni)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Dni))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Nombre == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
            return _context.Usuarios.Any(e => e.Nombre == id);
        }
    }
}
