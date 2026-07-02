using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Izmena
{
    public class IzmeniZaposlenogHandler
        : IRequestHandler<IzmeniZaposlenogKomanda, ZaposleniDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public IzmeniZaposlenogHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<ZaposleniDTO> Handle(
            IzmeniZaposlenogKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var zaposleni =
                await _jedinicaRada.Zaposleni.VratiPoId(zahtev.Id);

            if (zaposleni is null)
            {
                throw new KeyNotFoundException(
                    $"Zaposleni sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var sviZaposleni =
                await _jedinicaRada.Zaposleni.VratiSve();

            bool imejlKoristiDrugiZaposleni = sviZaposleni.Any(z =>
                z.Id != zahtev.Id &&
                z.Imejl.Equals(
                    zahtev.Podaci.Imejl,
                    StringComparison.OrdinalIgnoreCase));

            if (imejlKoristiDrugiZaposleni)
            {
                throw new InvalidOperationException(
                    "Drugi zaposleni već koristi unetu imejl adresu.");
            }

            zaposleni.Ime = zahtev.Podaci.Ime;
            zaposleni.Prezime = zahtev.Podaci.Prezime;
            zaposleni.Imejl = zahtev.Podaci.Imejl;
            zaposleni.RadnoMesto = zahtev.Podaci.RadnoMesto;
            zaposleni.Odeljenje = zahtev.Podaci.Odeljenje;
            zaposleni.DatumZaposlenja = zahtev.Podaci.DatumZaposlenja;
            zaposleni.Aktivan = zahtev.Podaci.Aktivan;

            _jedinicaRada.Zaposleni.Izmeni(zaposleni);
            await _jedinicaRada.SacuvajPromene();

            return new ZaposleniDTO
            {
                Id = zaposleni.Id,
                Ime = zaposleni.Ime,
                Prezime = zaposleni.Prezime,
                Imejl = zaposleni.Imejl,
                RadnoMesto = zaposleni.RadnoMesto,
                Odeljenje = zaposleni.Odeljenje,
                DatumZaposlenja = zaposleni.DatumZaposlenja,
                Aktivan = zaposleni.Aktivan
            };
        }
    }
}