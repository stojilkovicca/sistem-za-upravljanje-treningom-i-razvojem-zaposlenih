using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.Domen.Interfejsi;
using ZaposleniEntitet = TreningIRazvoj.Domen.Modeli.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Kreiranje
{
    public class KreirajZaposlenogHandler
        : IRequestHandler<KreirajZaposlenogKomanda, ZaposleniDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public KreirajZaposlenogHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<ZaposleniDTO> Handle(
            KreirajZaposlenogKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var sviZaposleni = await _jedinicaRada.Zaposleni.VratiSve();

            bool imejlPostoji = sviZaposleni.Any(z =>
                z.Imejl.Equals(
                    zahtev.Podaci.Imejl,
                    StringComparison.OrdinalIgnoreCase));

            if (imejlPostoji)
            {
                throw new InvalidOperationException(
                    "Zaposleni sa unetom imejl adresom već postoji.");
            }

            var zaposleni = new ZaposleniEntitet
            {
                Ime = zahtev.Podaci.Ime,
                Prezime = zahtev.Podaci.Prezime,
                Imejl = zahtev.Podaci.Imejl,
                RadnoMesto = zahtev.Podaci.RadnoMesto,
                Odeljenje = zahtev.Podaci.Odeljenje,
                DatumZaposlenja = zahtev.Podaci.DatumZaposlenja,
                Aktivan = zahtev.Podaci.Aktivan
            };

            await _jedinicaRada.Zaposleni.Dodaj(zaposleni);
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
