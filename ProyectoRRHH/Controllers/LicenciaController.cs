﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;
using ProyectoRRHH.Models;

namespace ProyectoRRHH.Controllers
{
    public class LicenciaController : Controller
    {
        private readonly EmpresaDatabaseContext _context;

        public LicenciaController(EmpresaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Licencia
        public async Task<IActionResult> Index(string buscar, string filtro)
        {
            // Persistir parámetros en ViewData
            ViewData["Buscar"] = buscar;
            // Obtener el DNI del usuario logueado
            var usuarioDni = User.FindFirstValue("Dni");
            if (User.IsInRole("Empleado"))
            {
                // Filtrar las licencias solo para el usuario logueado
                var licencias = await _context.Licencias
                                            .Where(l => l.UsuarioDni == usuarioDni)
                                            .Include(l => l.Usuario)
                                            .ToListAsync();

                // Pasar el DNI a la vista
                ViewBag.UsuarioDni = usuarioDni;

                return View(licencias);
            }
            else if (User.IsInRole("Administrador"))
            {
                var licencias = _context.Licencias.Include(l => l.Usuario).AsQueryable();

                if (!string.IsNullOrEmpty(buscar))
                {
                    licencias = licencias.Where(s => new[] { s.Usuario.Nombre, s.Usuario.Apellido }.Any(a => a.Contains(buscar)));
                }

                if (string.IsNullOrEmpty(filtro))
                {
                    filtro = "FechaIniDescendente"; // Valor por defecto si no se pasa filtro
                }

                ViewData["FiltroNombre"] = filtro == "NombreAscendente" ? "NombreDescendente" : "NombreAscendente";
                ViewData["FiltroEstado"] = filtro == "EstadoAscendente" ? "EstadoDescendente" : "EstadoAscendente";
                ViewData["FiltroFechaIni"] = filtro == "FechaIniAscendente" ? "FechaIniDescendente" : "FechaIniAscendente";

                switch (filtro)
                {
                    case "NombreDescendente":
                        licencias = licencias.OrderByDescending(licencia => licencia.Usuario.Nombre);
                        break;
                    case "FechaIniDescendente":
                        licencias = licencias.OrderByDescending(licencia => licencia.FechaInicio);
                        break;
                    case "FechaIniAscendente":
                        licencias = licencias.OrderBy(licencia => licencia.FechaInicio);
                        break;
                    case "EstadoDescendente":
                        licencias = licencias.OrderByDescending(licencia => licencia.Aprobado);
                        break;
                    case "EstadoAscendente":
                        licencias = licencias.OrderBy(licencia => licencia.Aprobado);
                        break;
                    default:
                        licencias = licencias.OrderBy(licencia => licencia.Usuario.Nombre);
                        break;
                }

                // Convertir a lista y devolver
                var listaLicencias = await licencias.ToListAsync();
                return View(licencias);
            }
            else
            {
                // Si el usuario no tiene el rol adecuado
                return Unauthorized();
            }
        }

        // GET: Licencia/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencia = await _context.Licencias
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licencia == null)
            {
                return NotFound();
            }

            return View(licencia);
        }

        // GET: Licencia/Create
        public IActionResult Create()
        {
            // Lista de DNI para el dropdown
            //ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni");
            
            if(User.IsInRole("Empleado"))
            {
                var usuarioDni = User.FindFirstValue("Dni");
                // Pasar el DNI a la vista
                var nombreCompleto = User.FindFirstValue("Nombre") + " " + User.FindFirstValue("Apellido");
                Usuario u = _context.Usuarios.FirstOrDefault(u => u.Dni == usuarioDni);
                ViewBag.UsuarioDni = usuarioDni;
                ViewBag.NombreCompleto = u.Nombre + " " + u.Apellido;
            }

            if(User.IsInRole("Administrador"))
            {
                // Lista de DNI para el dropdown
                ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni");

                // Diccionario con la información de los usuarios: DNI como clave y "Nombre Apellido" como valor
                ViewBag.UsuariosInfo = _context.Usuarios
                    .Select(u => new { u.Dni, NombreCompleto = $"{u.Nombre} {u.Apellido}" })
                    .ToDictionary(u => u.Dni, u => u.NombreCompleto);
            }
            return View();
        }

        public IActionResult SetAprobadoTrue(Guid? id)
        {
            var licencia = _context.Licencias.Find(id);
            if (licencia != null)
            {
                licencia.Aprobado = true;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetAprobadoFalse(Guid? id)
        {
            var licencia = _context.Licencias.Find(id);
            if (licencia != null)
            {
                licencia.Aprobado = false;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RevertAprobado(Guid? id)
        {
            var licencia = _context.Licencias.Find(id);
            if (licencia != null)
            {
                licencia.Aprobado = null;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Licencia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaInicio,CantDias,UsuarioDni,Aprobado")] Licencia licencia)
        {
            
            if(User.IsInRole("Administrador"))
            {
                // Que sea falso el Aprobado null, ya que si está marcado devuelve true, sino, null por Licencia/Create
                if (licencia.Aprobado == null)
                    licencia.Aprobado = false;
            }
            if (ModelState.IsValid)
            {
                licencia.Id = Guid.NewGuid();
                _context.Add(licencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", licencia.UsuarioDni);
            return View(licencia);
        }



        // GET: Licencia/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencia = await _context.Licencias.FindAsync(id);
            if (licencia == null)
            {
                return NotFound();
            }
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", licencia.UsuarioDni);
            return View(licencia);
        }

        // POST: Licencia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FechaInicio,CantDias,UsuarioDni,Aprobado")] Licencia licencia)
        {
            if (id != licencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenciaExists(licencia.Id))
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
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", licencia.UsuarioDni);
            return View(licencia);
        }

        // GET: Licencia/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencia = await _context.Licencias
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licencia == null)
            {
                return NotFound();
            }

            return View(licencia);
        }

        // POST: Licencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var licencia = await _context.Licencias.FindAsync(id);
            if (licencia != null)
            {
                _context.Licencias.Remove(licencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenciaExists(Guid id)
        {
            return _context.Licencias.Any(e => e.Id == id);
        }
    }
}
