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
    public class ReciboSueldoController : Controller
    {
        private readonly EmpresaDatabaseContext _context;

        public ReciboSueldoController(EmpresaDatabaseContext context)
        {
            _context = context;
        }

        // GET: ReciboSueldoes
        public async Task<IActionResult> Index()
        {
            var empresaDatabaseContext = _context.ReciboSueldos.Include(r => r.Usuario);
            return View(await empresaDatabaseContext.ToListAsync());
        }

        // GET: ReciboSueldoes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reciboSueldo = await _context.ReciboSueldos
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reciboSueldo == null)
            {
                return NotFound();
            }

            return View(reciboSueldo);
        }

        // GET: ReciboSueldoes/Create
        public IActionResult Create()
        {
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni");
            return View();
        }

        // POST: ReciboSueldoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCobro,sueldo,UsuarioDni")] ReciboSueldo reciboSueldo)
        {
            if (ModelState.IsValid)
            {
                reciboSueldo.Id = Guid.NewGuid();
                _context.Add(reciboSueldo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", reciboSueldo.UsuarioDni);
            return View(reciboSueldo);
        }

        // GET: ReciboSueldoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reciboSueldo = await _context.ReciboSueldos.FindAsync(id);
            if (reciboSueldo == null)
            {
                return NotFound();
            }
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", reciboSueldo.UsuarioDni);
            return View(reciboSueldo);
        }

        // POST: ReciboSueldoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FechaCobro,sueldo,UsuarioDni")] ReciboSueldo reciboSueldo)
        {
            if (id != reciboSueldo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reciboSueldo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReciboSueldoExists(reciboSueldo.Id))
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
            ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni", reciboSueldo.UsuarioDni);
            return View(reciboSueldo);
        }

        // GET: ReciboSueldoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reciboSueldo = await _context.ReciboSueldos
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reciboSueldo == null)
            {
                return NotFound();
            }

            return View(reciboSueldo);
        }

        // POST: ReciboSueldoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reciboSueldo = await _context.ReciboSueldos.FindAsync(id);
            if (reciboSueldo != null)
            {
                _context.ReciboSueldos.Remove(reciboSueldo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReciboSueldoExists(Guid id)
        {
            return _context.ReciboSueldos.Any(e => e.Id == id);
        }
    }
}
