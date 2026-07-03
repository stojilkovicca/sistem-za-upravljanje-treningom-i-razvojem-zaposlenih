using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Predavaci;
using TreningIRazvoj.Frontend.Servisi;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class PredavaciController : Controller
    {
        private readonly IApiServis _apiServis;

        public PredavaciController(IApiServis apiServis)
        {
            _apiServis = apiServis;
        }

        public async Task<IActionResult> Index()
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.GetAsync("/api/predavaci");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska = "Greška pri učitavanju predavača.";
                return View(new List<PredavacModel>());
            }

            var predavaci = await odgovor.Content
                .ReadFromJsonAsync<List<PredavacModel>>();

            return View(predavaci ?? new());
        }

        [HttpGet]
        public IActionResult Kreiraj()
        {
            return View(new PredavacModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(PredavacModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/predavaci",
                new
                {
                    model.Ime,
                    model.Prezime,
                    model.Imejl,
                    model.OblastStrucnosti,
                    model.Interni
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Predavač nije kreiran.");

                return View(model);
            }

            TempData["Poruka"] = "Predavač je uspešno kreiran.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                $"/api/predavaci/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] = "Predavač nije pronađen.";
                return RedirectToAction(nameof(Index));
            }

            var model = await odgovor.Content
                .ReadFromJsonAsync<PredavacModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(
            int id,
            PredavacModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PutAsJsonAsync(
                $"/api/predavaci/{id}",
                new
                {
                    model.Ime,
                    model.Prezime,
                    model.Imejl,
                    model.OblastStrucnosti,
                    model.Interni
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Izmena predavača nije uspela.");

                return View(model);
            }

            TempData["Poruka"] = "Predavač je uspešno izmenjen.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.DeleteAsync(
                $"/api/predavaci/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "Brisanje nije uspelo. Predavač se možda koristi.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Poruka"] = "Predavač je uspešno obrisan.";

            return RedirectToAction(nameof(Index));
        }

        private IActionResult? ObradiAutorizaciju(
            HttpResponseMessage odgovor)
        {
            if (odgovor.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();

                return RedirectToAction(
                    "Prijava",
                    "Autentifikacija");
            }

            if (odgovor.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Greska"] =
                    "Nemate dozvolu za ovu funkcionalnost.";

                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            return null;
        }
    }
}