using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Prijave;
using TreningIRazvoj.Frontend.Models.RazvojniProgrami;
using TreningIRazvoj.Frontend.Models.Zaposleni;
using TreningIRazvoj.Frontend.Servisi;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class PrijaveController : Controller
    {
        private readonly IApiServis _apiServis;

        public PrijaveController(IApiServis apiServis)
        {
            _apiServis = apiServis;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                "/api/prijave");

            var preusmerenje =
                ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška prilikom učitavanja prijava.";

                return View(new List<PrijavaModel>());
            }

            var prijave = await odgovor.Content
                .ReadFromJsonAsync<List<PrijavaModel>>();

            return View(prijave ?? []);
        }

        [HttpGet]
        public async Task<IActionResult> Kreiraj()
        {
            var model = new KreirajPrijavuModel();

            await UcitajSifarnike(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(
            KreirajPrijavuModel model)
        {
            if (!ModelState.IsValid)
            {
                await UcitajSifarnike(model);

                return View(model);
            }

            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/prijave",
                new
                {
                    model.ZaposleniId,
                    model.RazvojniProgramId
                });

            var preusmerenje =
                ObradiAutorizaciju(odgovor);

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
                        "Prijava nije kreirana."));

                await UcitajSifarnike(model);

                return View(model);
            }

            TempData["Poruka"] =
                "Prijava je uspešno kreirana.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Obradi(
            int zaposleniId,
            int razvojniProgramId)
        {
            var prijava = await VratiPrijavu(
                zaposleniId,
                razvojniProgramId);

            if (prijava is null)
            {
                TempData["Greska"] =
                    "Prijava nije pronađena.";

                return RedirectToAction(nameof(Index));
            }

            return View(new ObradiPrijavuModel
            {
                ZaposleniId = prijava.ZaposleniId,
                RazvojniProgramId =
                    prijava.RazvojniProgramId,

                ImeIPrezimeZaposlenog =
                    prijava.ImeIPrezimeZaposlenog,

                NazivRazvojnogPrograma =
                    prijava.NazivRazvojnogPrograma,

                Status = prijava.Status,
                ProcenatPrisustva =
                    prijava.ProcenatPrisustva,

                BrojPoena = prijava.BrojPoena,
                DatumZavrsetka =
                    prijava.DatumZavrsetka
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obradi(
            ObradiPrijavuModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var putanja =
                $"/api/prijave/{model.ZaposleniId}/" +
                $"{model.RazvojniProgramId}/obrada";

            var odgovor = await klijent.PutAsJsonAsync(
                putanja,
                new
                {
                    Status = (int)model.Status,
                    model.ProcenatPrisustva,
                    model.BrojPoena,
                    model.DatumZavrsetka
                });

            var preusmerenje =
                ObradiAutorizaciju(odgovor);

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
                        "Obrada prijave nije uspela."));

                return View(model);
            }

            TempData["Poruka"] =
                "Prijava je uspešno obrađena.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Oceni(
            int zaposleniId,
            int razvojniProgramId)
        {
            var prijava = await VratiPrijavu(
                zaposleniId,
                razvojniProgramId);

            if (prijava is null)
            {
                TempData["Greska"] =
                    "Prijava nije pronađena.";

                return RedirectToAction(nameof(Index));
            }

            return View(new OceniProgramModel
            {
                ZaposleniId = prijava.ZaposleniId,
                RazvojniProgramId =
                    prijava.RazvojniProgramId,

                ImeIPrezimeZaposlenog =
                    prijava.ImeIPrezimeZaposlenog,

                NazivRazvojnogPrograma =
                    prijava.NazivRazvojnogPrograma,

                OcenaPrograma =
                    prijava.OcenaPrograma ?? 5
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Oceni(
            OceniProgramModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var putanja =
                $"/api/prijave/{model.ZaposleniId}/" +
                $"{model.RazvojniProgramId}/ocena";

            var odgovor = await klijent.PutAsJsonAsync(
                putanja,
                new
                {
                    model.OcenaPrograma
                });

            var preusmerenje =
                ObradiAutorizaciju(odgovor);

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
                        "Ocenjivanje nije uspelo."));

                return View(model);
            }

            TempData["Poruka"] =
                "Program je uspešno ocenjen.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> PreuzmiIzvestaj()
        {
            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                "/api/prijave/izvestaj");

            var preusmerenje =
                ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "PDF izveštaj nije generisan.";

                return RedirectToAction(nameof(Index));
            }

            var pdf =
                await odgovor.Content.ReadAsByteArrayAsync();

            var nazivFajla =
                $"izvestaj-prijave-" +
                $"{DateTime.Now:yyyy-MM-dd-HH-mm}.pdf";

            return File(
                pdf,
                "application/pdf",
                nazivFajla);
        }

        private async Task<PrijavaModel?> VratiPrijavu(
            int zaposleniId,
            int razvojniProgramId)
        {
            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                $"/api/prijave/{zaposleniId}/" +
                $"{razvojniProgramId}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return null;
            }

            return await odgovor.Content
                .ReadFromJsonAsync<PrijavaModel>();
        }

        private async Task UcitajSifarnike(
            KreirajPrijavuModel model)
        {
            var klijent =
                _apiServis.KreirajKlijentaSaTokenom();

            var zaposleniOdgovor =
                await klijent.GetAsync("/api/zaposleni");

            var programiOdgovor =
                await klijent.GetAsync(
                    "/api/razvojni-programi");

            var zaposleni =
                zaposleniOdgovor.IsSuccessStatusCode
                    ? await zaposleniOdgovor.Content
                        .ReadFromJsonAsync<
                            List<ZaposleniModel>>() ?? []
                    : [];

            var programi =
                programiOdgovor.IsSuccessStatusCode
                    ? await programiOdgovor.Content
                        .ReadFromJsonAsync<
                            List<RazvojniProgramModel>>() ?? []
                    : [];

            model.Zaposleni = zaposleni
                .Where(z => z.Aktivan)
                .Select(z => new SelectListItem
                {
                    Value = z.Id.ToString(),
                    Text = $"{z.Ime} {z.Prezime}"
                })
                .ToList();

            model.RazvojniProgrami = programi
                .Where(p => p.Objavljen)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Naziv
                })
                .ToList();
        }

        private IActionResult? ObradiAutorizaciju(
            HttpResponseMessage odgovor)
        {
            if (odgovor.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();

                return RedirectToAction(
                    "Prijava",
                    "Autentifikacija");
            }

            if (odgovor.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                TempData["Greska"] =
                    "Nemate dozvolu za ovu funkcionalnost.";

                return RedirectToAction(
                    "Index",
                    "Pocetna");
            }

            return null;
        }

        private static async Task<string>
            ProcitajPorukuGreske(
                HttpResponseMessage odgovor,
                string podrazumevanaPoruka)
        {
            try
            {
                var podaci = await odgovor.Content
                    .ReadFromJsonAsync<
                        Dictionary<string, object>>();

                if (podaci is not null &&
                    podaci.TryGetValue(
                        "poruka",
                        out var poruka))
                {
                    return poruka?.ToString()
                        ?? podrazumevanaPoruka;
                }
            }
            catch
            {
                // Koristi se podrazumevana poruka.
            }

            return podrazumevanaPoruka;
        }
    }
}