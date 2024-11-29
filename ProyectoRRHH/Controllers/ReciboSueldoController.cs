using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;
using ProyectoRRHH.Models;
using System.Security.Claims;

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
        // Obtener el DNI del usuario logueado
        var usuarioDni = User.FindFirstValue("Dni");

        if (User.IsInRole("Empleado"))
        {
            // Filtrar los recibos solo para el usuario logueado
            var recibos = await _context.ReciboSueldos
                                        .Where(r => r.UsuarioDni == usuarioDni)
                                        .Include(r => r.Usuario)
                                        .ToListAsync();

            // Pasar el DNI a la vista
            ViewBag.UsuarioDni = usuarioDni;

            return View(recibos);
        }
        else if (User.IsInRole("Administrador"))
        {
           
            var recibos = await _context.ReciboSueldos.Include(r => r.Usuario).ToListAsync();
            return View(recibos);
        }
        else
        {
            // Si el usuario no tiene el rol adecuado
            return Unauthorized();
        }
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FechaCobro,sueldo,UsuarioDni")] ReciboSueldo reciboSueldo)
    {
        if (ModelState.IsValid)
        {
            reciboSueldo.Id = Guid.NewGuid();
            reciboSueldo.SueldoBruto = reciboSueldo.SueldoBruto * (1 - Constantes.Descuento);
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

