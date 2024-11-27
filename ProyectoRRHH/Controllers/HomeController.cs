using Microsoft.AspNetCore.Mvc;
using ProyectoRRHH.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoRRHH.Controllers
{

    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult AltaEmpleados()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult AltaRecibos()
        {
            return View();
        }

        [Authorize(Roles = "Empleado")]
        public IActionResult VistaRecibos()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
