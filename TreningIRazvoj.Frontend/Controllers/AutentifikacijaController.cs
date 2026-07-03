using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Autentifikacija;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class AutentifikacijaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AutentifikacijaController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Prijava()
        {
            if (!string.IsNullOrWhiteSpace(
                    HttpContext.Session.GetString("JwtToken")))
            {
                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            return View(new PrijavaKorisnikaModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prijava(
            PrijavaKorisnikaModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent = _httpClientFactory.CreateClient(
                "TreningIRazvojAPI");

            HttpResponseMessage odgovor;

            try
            {
                odgovor = await klijent.PostAsJsonAsync(
                    "/api/autentifikacija/prijava",
                    new
                    {
                        imejl = model.Imejl,
                        lozinka = model.Lozinka
                    });
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Nije moguće povezati se sa API aplikacijom. Proveri da li je API pokrenut.");

                return View(model);
            }

            if (odgovor.StatusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Imejl ili lozinka nisu ispravni.");

                return View(model);
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Došlo je do greške prilikom prijavljivanja.");

                return View(model);
            }

            var tokenPodaci =
                await odgovor.Content.ReadFromJsonAsync<TokenModel>();

            if (tokenPodaci is null ||
                string.IsNullOrWhiteSpace(tokenPodaci.Token))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "API nije vratio ispravan JWT token.");

                return View(model);
            }

            HttpContext.Session.SetString(
                "JwtToken",
                tokenPodaci.Token);

            HttpContext.Session.SetString(
                "Ime",
                tokenPodaci.Ime);

            HttpContext.Session.SetString(
                "Prezime",
                tokenPodaci.Prezime);

            HttpContext.Session.SetString(
                "Imejl",
                tokenPodaci.Imejl);

            HttpContext.Session.SetString(
                "Uloga",
                tokenPodaci.Uloge.FirstOrDefault()
                ?? string.Empty);

            return RedirectToAction(
                "Index",
                "Pocetna");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Odjava()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                nameof(Prijava));
        }

        [HttpGet]
        public IActionResult Registracija()
        {
            var uloga = HttpContext.Session.GetString("Uloga");

            if (uloga != "Administrator")
            {
                TempData["Greska"] =
                    "Samo administrator može da registruje nove korisnike.";

                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            return View(new RegistracijaKorisnikaModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registracija(
            RegistracijaKorisnikaModel model)
        {
            var uloga = HttpContext.Session.GetString("Uloga");

            if (uloga != "Administrator")
            {
                TempData["Greska"] =
                    "Samo administrator može da registruje nove korisnike.";

                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent = _httpClientFactory.CreateClient(
                "TreningIRazvojAPI");

            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                klijent.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/autentifikacija/registracija",
                new
                {
                    model.Ime,
                    model.Prezime,
                    model.Imejl,
                    model.Lozinka,
                    model.Uloga
                });

            if (odgovor.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();

                return RedirectToAction(nameof(Prijava));
            }

            if (odgovor.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Greska"] =
                    "Nemate dozvolu za registraciju korisnika.";

                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registracija korisnika nije uspela.");

                return View(model);
            }

            TempData["Poruka"] =
                $"Korisnik {model.Ime} {model.Prezime} je uspešno registrovan.";

            return RedirectToAction(
                "Index",
                "Pocetna");
        }








    }
}