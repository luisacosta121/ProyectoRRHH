using Microsoft.AspNetCore.Mvc;

using ProyectoRRHH.Models;
using ProyectoRRHH.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ProyectoRRHH.Controllers
{
    public class AccesoController : Controller
    {
        private readonly DA_Logica _da_Logica;

      
        public AccesoController(DA_Logica da_Logica)
        {
            _da_Logica = da_Logica;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            var usuario = _da_Logica.ValidarUsuario(_usuario.Correo, _usuario.Clave);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo),
                    new Claim("Dni", usuario.Dni),
                };

                foreach (string rol in usuario.Roles.Split(','))
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol.Trim()));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }
    }
}