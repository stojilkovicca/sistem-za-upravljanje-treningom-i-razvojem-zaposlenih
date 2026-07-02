using MediatR;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Brisanje
{
    public class ObrisiZaposlenogHandler
        : IRequestHandler<ObrisiZaposlenogKomanda, bool>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public ObrisiZaposlenogHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<bool> Handle(
            ObrisiZaposlenogKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var zaposleni =
                await _jedinicaRada.Zaposleni.VratiPoId(zahtev.Id);

            if (zaposleni is null)
            {
                throw new KeyNotFoundException(
                    $"Zaposleni sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var svePrijave =
                await _jedinicaRada.Prijave.VratiSve();

            bool imaPrijave = svePrijave.Any(
                p => p.ZaposleniId == zahtev.Id);

            if (imaPrijave)
            {
                throw new InvalidOperationException(
                    "Zaposleni se ne može obrisati jer ima evidentirane prijave. Možete ga označiti kao neaktivnog.");
            }

            _jedinicaRada.Zaposleni.Obrisi(zaposleni);
            await _jedinicaRada.SacuvajPromene();

            return true;
        }
    }
}
