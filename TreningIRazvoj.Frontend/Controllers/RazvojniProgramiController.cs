using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Json;
using TreningIRazvoj.Frontend.Models.Kategorije;
using TreningIRazvoj.Frontend.Models.Opste;
using TreningIRazvoj.Frontend.Models.Predavaci;
using TreningIRazvoj.Frontend.Models.RazvojniProgrami;
using TreningIRazvoj.Frontend.Servisi;

namespace TreningIRazvoj.Frontend.Controllers
{
    public class RazvojniProgramiController : Controller
    {
        private readonly IApiServis _apiServis;

        public RazvojniProgramiController(IApiServis apiServis)
        {
            _apiServis = apiServis;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? pretraga,
            VrstaProgramaModel? vrsta,
            int? kategorijaProgramaId,
            int? predavacId,
            bool? objavljen,
            string sortirajPo = "datumPocetka",
            string smerSortiranja = "rastuce",
            int brojStranice = 1,
            int velicinaStranice = 10)
        {
            var model = new PretragaRazvojnihProgramaModel
            {
                Pretraga = pretraga,
                Vrsta = vrsta,
                KategorijaProgramaId = kategorijaProgramaId,
                PredavacId = predavacId,
                Objavljen = objavljen,
                SortirajPo = sortirajPo,
                SmerSortiranja = smerSortiranja,
                BrojStranice = brojStranice,
                VelicinaStranice = velicinaStranice
            };

            await UcitajSifarnike(model);

            var parametri = new List<string>();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                parametri.Add(
                    $"pretraga={Uri.EscapeDataString(pretraga)}");
            }

            if (vrsta.HasValue)
            {
                parametri.Add($"vrsta={(int)vrsta.Value}");
            }

            if (kategorijaProgramaId.HasValue)
            {
                parametri.Add(
                    $"kategorijaProgramaId={kategorijaProgramaId}");
            }

            if (predavacId.HasValue)
            {
                parametri.Add($"predavacId={predavacId}");
            }

            if (objavljen.HasValue)
            {
                parametri.Add(
                    $"objavljen={objavljen.Value.ToString().ToLower()}");
            }

            parametri.Add(
                $"sortirajPo={Uri.EscapeDataString(sortirajPo)}");

            parametri.Add(
                $"smerSortiranja={Uri.EscapeDataString(smerSortiranja)}");

            parametri.Add($"brojStranice={brojStranice}");
            parametri.Add($"velicinaStranice={velicinaStranice}");

            var putanja =
                "/api/razvojni-programi/pretraga?" +
                string.Join("&", parametri);

            var klijent = _apiServis.KreirajKlijentaSaTokenom();
            var odgovor = await klijent.GetAsync(putanja);

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri učitavanju razvojnih programa.";

                return View(model);
            }

            model.Rezultat = await odgovor.Content
                .ReadFromJsonAsync<
                    StranicniRezultatModel<RazvojniProgramModel>>()
                ?? new();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Kreiraj()
        {
            await UcitajSifarnike();

            return View(new RazvojniProgramModel
            {
                DatumPocetka = DateTime.Today,
                DatumZavrsetka = DateTime.Today.AddDays(1),
                Kapacitet = 20,
                TrajanjeUSatima = 8
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kreiraj(
            RazvojniProgramModel model)
        {
            if (!ModelState.IsValid)
            {
                await UcitajSifarnike();
                return View(model);
            }

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PostAsJsonAsync(
                "/api/razvojni-programi",
                KreirajPodatkeZaSlanje(model));

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Razvojni program nije kreiran.");

                await UcitajSifarnike();

                return View(model);
            }

            TempData["Poruka"] =
                "Razvojni program je uspešno kreiran.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.GetAsync(
                $"/api/razvojni-programi/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "Razvojni program nije pronađen.";

                return RedirectToAction(nameof(Index));
            }

            var model = await odgovor.Content
                .ReadFromJsonAsync<RazvojniProgramModel>();

            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            await UcitajSifarnike();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmeni(
            int id,
            RazvojniProgramModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await UcitajSifarnike();
                return View(model);
            }

            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.PutAsJsonAsync(
                $"/api/razvojni-programi/{id}",
                KreirajPodatkeZaSlanje(model));

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Izmena razvojnog programa nije uspela.");

                await UcitajSifarnike();

                return View(model);
            }

            TempData["Poruka"] =
                "Razvojni program je uspešno izmenjen.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var odgovor = await klijent.DeleteAsync(
                $"/api/razvojni-programi/{id}");

            var preusmerenje = ObradiAutorizaciju(odgovor);

            if (preusmerenje is not null)
            {
                return preusmerenje;
            }

            if (!odgovor.IsSuccessStatusCode)
            {
                TempData["Greska"] =
                    "Brisanje nije uspelo. Program možda ima prijave.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Poruka"] =
                "Razvojni program je uspešno obrisan.";

            return RedirectToAction(nameof(Index));
        }

        private static object KreirajPodatkeZaSlanje(
            RazvojniProgramModel model)
        {
            return new
            {
                model.Naziv,
                model.Opis,
                Vrsta = (int)model.Vrsta,
                model.DatumPocetka,
                model.DatumZavrsetka,
                model.Kapacitet,
                model.TrajanjeUSatima,
                model.MinimalanBrojPoena,
                model.Objavljen,
                model.KategorijaProgramaId,
                model.PredavacId
            };
        }

        private async Task UcitajSifarnike(
            PretragaRazvojnihProgramaModel? model = null)
        {
            var klijent = _apiServis.KreirajKlijentaSaTokenom();

            var kategorijeOdgovor = await klijent.GetAsync(
                "/api/kategorije-programa");

            var predavaciOdgovor = await klijent.GetAsync(
                "/api/predavaci");

            var kategorije = kategorijeOdgovor.IsSuccessStatusCode
                ? await kategorijeOdgovor.Content
                    .ReadFromJsonAsync<List<KategorijaProgramaModel>>()
                    ?? []
                : [];

            var predavaci = predavaciOdgovor.IsSuccessStatusCode
                ? await predavaciOdgovor.Content
                    .ReadFromJsonAsync<List<PredavacModel>>()
                    ?? []
                : [];

            var stavkeKategorija = kategorije.Select(k =>
                new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = k.Naziv
                }).ToList();

            var stavkePredavaca = predavaci.Select(p =>
                new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Ime} {p.Prezime}"
                }).ToList();

            ViewBag.Kategorije = stavkeKategorija;
            ViewBag.Predavaci = stavkePredavaca;

            if (model is not null)
            {
                model.Kategorije = stavkeKategorija;
                model.Predavaci = stavkePredavaca;
            }
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