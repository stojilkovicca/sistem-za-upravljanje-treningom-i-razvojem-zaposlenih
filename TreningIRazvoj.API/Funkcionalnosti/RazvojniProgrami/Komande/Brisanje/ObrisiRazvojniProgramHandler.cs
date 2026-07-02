using MediatR;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Brisanje
{
    public class ObrisiRazvojniProgramHandler
        : IRequestHandler<ObrisiRazvojniProgramKomanda, bool>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public ObrisiRazvojniProgramHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<bool> Handle(
            ObrisiRazvojniProgramKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(zahtev.Id);

            if (program is null)
            {
                throw new KeyNotFoundException(
                    $"Razvojni program sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var svePrijave =
                await _jedinicaRada.Prijave.VratiSve();

            bool programImaPrijave = svePrijave.Any(p =>
                p.RazvojniProgramId == zahtev.Id);

            if (programImaPrijave)
            {
                throw new InvalidOperationException(
                    "Razvojni program se ne može obrisati jer postoje evidentirane prijave za njega.");
            }

            _jedinicaRada.RazvojniProgrami.Obrisi(program);
            await _jedinicaRada.SacuvajPromene();

            return true;
        }
    }
}