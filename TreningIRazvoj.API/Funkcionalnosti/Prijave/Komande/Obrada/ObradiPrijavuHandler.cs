using MediatR;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.Domen.Enumeracije;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Obrada
{
    public class ObradiPrijavuHandler
        : IRequestHandler<ObradiPrijavuKomanda, PrijavaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public ObradiPrijavuHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PrijavaDTO> Handle(
            ObradiPrijavuKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var prijava =
                await _jedinicaRada.Prijave.VratiPoId(
                    zahtev.ZaposleniId,
                    zahtev.RazvojniProgramId);

            if (prijava is null)
            {
                throw new KeyNotFoundException(
                    $"Prijava zaposlenog {zahtev.ZaposleniId} za razvojni program {zahtev.RazvojniProgramId} nije pronađena.");
            }

            var zaposleni =
                await _jedinicaRada.Zaposleni
                    .VratiPoId(zahtev.ZaposleniId);

            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(zahtev.RazvojniProgramId);

            if (zaposleni is null || program is null)
            {
                throw new InvalidOperationException(
                    "Podaci povezani sa prijavom nisu pronađeni.");
            }

            if (zahtev.Podaci.Status == StatusPrijave.NaCekanju)
            {
                throw new InvalidOperationException(
                    "Prijava se ne može ponovo postaviti na status NaCekanju.");
            }

            if (prijava.Status == StatusPrijave.Zavrsena)
            {
                throw new InvalidOperationException(
                    "Završena prijava se više ne može obrađivati.");
            }

            if (prijava.Status == StatusPrijave.Odbijena)
            {
                throw new InvalidOperationException(
                    "Odbijena prijava se više ne može obrađivati.");
            }

            if (prijava.Status == StatusPrijave.Otkazana)
            {
                throw new InvalidOperationException(
                    "Otkazana prijava se više ne može obrađivati.");
            }

            if (zahtev.Podaci.Status == StatusPrijave.Odobrena &&
                prijava.Status != StatusPrijave.NaCekanju)
            {
                throw new InvalidOperationException(
                    "Samo prijava na čekanju može biti odobrena.");
            }

            if (zahtev.Podaci.Status == StatusPrijave.Odbijena &&
                prijava.Status != StatusPrijave.NaCekanju)
            {
                throw new InvalidOperationException(
                    "Samo prijava na čekanju može biti odbijena.");
            }

            if (zahtev.Podaci.Status == StatusPrijave.Zavrsena)
            {
                if (prijava.Status != StatusPrijave.Odobrena)
                {
                    throw new InvalidOperationException(
                        "Samo odobrena prijava može biti označena kao završena.");
                }

                if (!zahtev.Podaci.ProcenatPrisustva.HasValue)
                {
                    throw new InvalidOperationException(
                        "Za završenu prijavu mora biti unet procenat prisustva.");
                }

                if (!zahtev.Podaci.BrojPoena.HasValue)
                {
                    throw new InvalidOperationException(
                        "Za završenu prijavu mora biti unet broj poena.");
                }

                if (!zahtev.Podaci.DatumZavrsetka.HasValue)
                {
                    throw new InvalidOperationException(
                        "Za završenu prijavu mora biti unet datum završetka.");
                }

                if (zahtev.Podaci.DatumZavrsetka.Value <
                    program.DatumPocetka)
                {
                    throw new InvalidOperationException(
                        "Datum završetka prijave ne može biti pre početka programa.");
                }
            }

            prijava.Status = zahtev.Podaci.Status;
            prijava.ProcenatPrisustva =
                zahtev.Podaci.ProcenatPrisustva;
            prijava.BrojPoena = zahtev.Podaci.BrojPoena;
            prijava.DatumZavrsetka =
                zahtev.Podaci.DatumZavrsetka;

            _jedinicaRada.Prijave.Izmeni(prijava);
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