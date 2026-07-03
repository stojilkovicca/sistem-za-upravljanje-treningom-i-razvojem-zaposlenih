using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Kategorije;
using TreningIRazvoj.Frontend.Servisi;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class KategorijeController : Controller
    {
        private readonly IApiServis _apiServis;

        public KategorijeController(IApiServis apiServis)
        {
            _apiServis = apiServis;
        }

        public async Task<IActionResult> Index()
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.GetAsync("/api/kategorije-programa");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska = "Greška pri učitavanju kategorija.";
                return View(new List<KategorijaProgramaModel>());
            }

            var kategorije = await odgovor.Content
                .ReadFromJsonAsync<List<KategorijaProgramaModel>>();

            return View(kategorije ?? new());
        }

        [HttpGet]
        public IActionResult Kreiraj()
        {
            return View(new KategorijaProgramaModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(
            KategorijaProgramaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/kategorije-programa",
                new
                {
                    model.Naziv,
                    model.Opis
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Kategorija nije kreirana.");

                return View(model);
            }

            TempData["Poruka"] = "Kategorija je uspešno kreirana.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                $"/api/kategorije-programa/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] = "Kategorija nije pronađena.";
                return RedirectToAction(nameof(Index));
            }

            var model = await odgovor.Content
                .ReadFromJsonAsync<KategorijaProgramaModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(
            int id,
            KategorijaProgramaModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PutAsJsonAsync(
                $"/api/kategorije-programa/{id}",
                new
                {
                    model.Naziv,
                    model.Opis
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Izmena kategorije nije uspela.");

                return View(model);
            }

            TempData["Poruka"] = "Kategorija je uspešno izmenjena.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.DeleteAsync(
                $"/api/kategorije-programa/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
                return preusmerenje;

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "Brisanje nije uspelo. Kategorija se možda koristi.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Poruka"] = "Kategorija je uspešno obrisana.";

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