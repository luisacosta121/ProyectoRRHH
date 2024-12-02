using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;
using ProyectoRRHH.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Elfie.Serialization;

public class ReciboSueldoController : Controller
{
    private readonly EmpresaDatabaseContext _context;

    public ReciboSueldoController(EmpresaDatabaseContext context)
    {
        _context = context;
    }

    // GET: ReciboSueldoes
    public async Task<IActionResult> Index(string buscar, string filtro)
    {
        // Persistir parámetros en ViewData
        ViewData["Buscar"] = buscar;

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
            // Obtener todos los recibos
            var recibos = _context.ReciboSueldos.Include(r => r.Usuario).AsQueryable();

            // Aplicar búsqueda si es necesario
            if (!string.IsNullOrEmpty(buscar))
            {
                recibos = recibos.Where(s => s.UsuarioDni.Contains(buscar));
            }

            // Establecer el filtro de ordenación (alternar entre "DniAscendente" y "DniDescendente")
            if (string.IsNullOrEmpty(filtro))
            {
                filtro = "DniAscendente"; // Valor por defecto si no se pasa filtro
            }

            // Alternar entre ascendente y descendente
            ViewData["FiltroDni"] = filtro == "DniAscendente" ? "DniDescendente" : "DniAscendente";

            // Aplicar orden según el filtro
            switch (filtro)
            {
                case "DniDescendente":
                    recibos = recibos.OrderByDescending(r => r.UsuarioDni);
                    break;

                case "DniAscendente":
                default:
                    recibos = recibos.OrderBy(r => r.UsuarioDni);
                    break;
            }

            // Convertir a lista y devolver
            var listaRecibos = await recibos.ToListAsync();
            return View(listaRecibos);
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
        // Lista de DNI para el dropdown
        ViewData["UsuarioDni"] = new SelectList(_context.Usuarios, "Dni", "Dni");

        // Diccionario con la información de los usuarios: DNI como clave y "Nombre Apellido" como valor
        ViewBag.UsuariosInfo = _context.Usuarios
            .Select(u => new { u.Dni, NombreCompleto = $"{u.Nombre} {u.Apellido}" })
            .ToDictionary(u => u.Dni, u => u.NombreCompleto);

        return View();
    }

    // POST: ReciboSueldoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FechaCobro,SueldoBruto,UsuarioDni")] ReciboSueldo reciboSueldo)
    {
        if (ModelState.IsValid)
        {
            reciboSueldo.Id = Guid.NewGuid();
            reciboSueldo.SueldoBruto = reciboSueldo.SueldoBruto;
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
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,FechaCobro,SueldoBruto,UsuarioDni")] ReciboSueldo reciboSueldo)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Firmar(Guid id)
    {
        // Obtener el recibo
        var recibo = await _context.ReciboSueldos.FindAsync(id);

        if (recibo == null)
        {
            return NotFound();
        }

        // Verificar que el recibo pertenece al usuario logueado
        var usuarioDni = User.FindFirstValue("Dni");
        if (recibo.UsuarioDni != usuarioDni)
        {
            return Unauthorized();
        }

        // Verificar si ya está firmado
        if (recibo.Firmado)
        {
            return BadRequest("El recibo ya está firmado.");
        }

        // Marcar el recibo como firmado
        recibo.Firmado = true;

        _context.Update(recibo);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ReciboSueldoExists(Guid id)
    {
        return _context.ReciboSueldos.Any(e => e.Id == id);
    }
}

