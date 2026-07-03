using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Zaposleni;
using TreningIRazvoj.Frontend.Servisi;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class ZaposleniController : Controller
    {
        private readonly IApiServis _apiServis;

        public ZaposleniController(IApiServis apiServis)
        {
            _apiServis = apiServis;
        }

        public async Task<IActionResult> Index()
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.GetAsync("/api/zaposleni");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Došlo je do greške prilikom učitavanja zaposlenih.";

                return View(new List<ZaposleniModel>());
            }

            var zaposleni =
                await odgovor.Content.ReadFromJsonAsync<List<ZaposleniModel>>();

            return View(zaposleni ?? new List<ZaposleniModel>());
        }

        [HttpGet]
        public IActionResult Kreiraj()
        {
            if (!KorisnikJePrijavljen())
            {
                return RedirectNaPrijavu();
            }

            return View(new ZaposleniModel
            {
                DatumZaposlenja = DateTime.Today,
                Aktivan = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(ZaposleniModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/zaposleni",
                new
                {
                    model.Ime,
                    model.Prezime,
                    model.Imejl,
                    model.RadnoMesto,
                    model.Odeljenje,
                    model.DatumZaposlenja,
                    model.Aktivan
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    await ProcitajPorukuGreske(
                        odgovor,
                        "Zaposleni nije kreiran."));

                return View(model);
            }

            TempData["Poruka"] =
                "Zaposleni je uspešno kreiran.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.GetAsync($"/api/zaposleni/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (odgovor.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["Greska"] = "Zaposleni nije pronađen.";

                return RedirectToAction(nameof(Index));
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "Došlo je do greške prilikom učitavanja zaposlenog.";

                return RedirectToAction(nameof(Index));
            }

            var zaposleni =
                await odgovor.Content.ReadFromJsonAsync<ZaposleniModel>();

            if (zaposleni is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(zaposleni);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(
            int id,
            ZaposleniModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PutAsJsonAsync(
                $"/api/zaposleni/{id}",
                new
                {
                    model.Ime,
                    model.Prezime,
                    model.Imejl,
                    model.RadnoMesto,
                    model.Odeljenje,
                    model.DatumZaposlenja,
                    model.Aktivan
                });

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    await ProcitajPorukuGreske(
                        odgovor,
                        "Izmena nije uspešna."));

                return View(model);
            }

            TempData["Poruka"] =
                "Podaci zaposlenog su uspešno izmenjeni.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.DeleteAsync(
                $"/api/zaposleni/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    await ProcitajPorukuGreske(
                        odgovor,
                        "Brisanje zaposlenog nije uspelo.");

                return RedirectToAction(nameof(Index));
            }

            TempData["Poruka"] =
                "Zaposleni je uspešno obrisan.";

            return RedirectToAction(nameof(Index));
        }

        private IActionResult? ObradiAutorizaciju(
            HttpResponseMessage odgovor)
        {
            if (odgovor.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();

                return RedirectNaPrijavu();
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

        private bool KorisnikJePrijavljen()
        {
            return !string.IsNullOrWhiteSpace(
                HttpContext.Session.GetString("JwtToken"));
        }

        private IActionResult RedirectNaPrijavu()
        {
            return RedirectToAction(
                "Prijava",
                "Autentifikacija");
        }

        private static async Task<string> ProcitajPorukuGreske(
            HttpResponseMessage odgovor,
            string podrazumevanaPoruka)
        {
            try
            {
                var podaci =
                    await odgovor.Content.ReadFromJsonAsync<
                        Dictionary<string, object>>();

                if (podaci is not null &&
                    podaci.TryGetValue("poruka", out var poruka))
                {
                    return poruka?.ToString()
                        ?? podrazumevanaPoruka;
                }
            }
            catch
            {
                // Ako odgovor nije JSON, vraća se podrazumevana poruka.
            }

            return podrazumevanaPoruka;
        }
    }
}