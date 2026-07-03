using Microsoft.AspNetCore.Mvc;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class PocetnaController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(
                    "Prijava",
                    "Autentifikacija");
            }

            ViewBag.Ime = HttpContext.Session.GetString("Ime");
            ViewBag.Prezime = HttpContext.Session.GetString("Prezime");
            ViewBag.Imejl = HttpContext.Session.GetString("Imejl");
            ViewBag.Uloga = HttpContext.Session.GetString("Uloga");

            return View();
        }
    }
}