using MediatR;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.Domen.Enumeracije;
using TreningIRazvoj.Domen.Interfejsi;
using PrijavaEntitet = TreningIRazvoj.Domen.Modeli.Prijava;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Kreiranje
{
    public class KreirajPrijavuHandler
        : IRequestHandler<KreirajPrijavuKomanda, PrijavaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public KreirajPrijavuHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PrijavaDTO> Handle(
            KreirajPrijavuKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var zaposleni =
                await _jedinicaRada.Zaposleni
                    .VratiPoId(zahtev.Podaci.ZaposleniId);

            if (zaposleni is null)
            {
                throw new KeyNotFoundException(
                    $"Zaposleni sa identifikatorom {zahtev.Podaci.ZaposleniId} nije pronađen.");
            }

            if (!zaposleni.Aktivan)
            {
                throw new InvalidOperationException(
                    "Neaktivan zaposleni ne može da se prijavi na razvojni program.");
            }

            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(zahtev.Podaci.RazvojniProgramId);

            if (program is null)
            {
                throw new KeyNotFoundException(
                    $"Razvojni program sa identifikatorom {zahtev.Podaci.RazvojniProgramId} nije pronađen.");
            }

            if (!program.Objavljen)
            {
                throw new InvalidOperationException(
                    "Prijavljivanje na razvojni program koji nije objavljen nije dozvoljeno.");
            }

            if (program.DatumPocetka <= DateTime.Now)
            {
                throw new InvalidOperationException(
                    "Prijavljivanje nije dozvoljeno jer je razvojni program već počeo.");
            }

            var svePrijave =
                await _jedinicaRada.Prijave.VratiSve();

            bool prijavaVecPostoji = svePrijave.Any(p =>
                p.ZaposleniId == zahtev.Podaci.ZaposleniId &&
                p.RazvojniProgramId == zahtev.Podaci.RazvojniProgramId);

            if (prijavaVecPostoji)
            {
                throw new InvalidOperationException(
                    "Zaposleni je već prijavljen na ovaj razvojni program.");
            }

            int brojAktivnihPrijava = svePrijave.Count(p =>
                p.RazvojniProgramId == zahtev.Podaci.RazvojniProgramId &&
                p.Status != StatusPrijave.Odbijena &&
                p.Status != StatusPrijave.Otkazana);

            if (brojAktivnihPrijava >= program.Kapacitet)
            {
                throw new InvalidOperationException(
                    "Kapacitet razvojnog programa je popunjen.");
            }

            var prijava = new PrijavaEntitet
            {
                ZaposleniId = zaposleni.Id,
                RazvojniProgramId = program.Id,
                DatumPrijave = DateTime.Now,
                Status = StatusPrijave.NaCekanju,
                ProcenatPrisustva = null,
                BrojPoena = null,
                DatumZavrsetka = null,
                OcenaPrograma = null
            };

            await _jedinicaRada.Prijave.Dodaj(prijava);
            await _jedinicaRada.SacuvajPromene();

            return new PrijavaDTO
            {
                ZaposleniId = prijava.ZaposleniId,
                ImeIPrezimeZaposlenog =
                    $"{zaposleni.Ime} {zaposleni.Prezime}",
                RazvojniProgramId = prijava.RazvojniProgramId,
                NazivRazvojnogPrograma = program.Naziv,
                DatumPrijave = prijava.DatumPrijave,
                Status = prijava.Status,
                ProcenatPrisustva = prijava.ProcenatPrisustva,
                BrojPoena = prijava.BrojPoena,
                DatumZavrsetka = prijava.DatumZavrsetka,
                OcenaPrograma = prijava.OcenaPrograma
            };
        }
    }
}