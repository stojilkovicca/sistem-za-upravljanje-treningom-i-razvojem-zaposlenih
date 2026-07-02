using MediatR;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Brisanje
{
    public class ObrisiPredavacaHandler
        : IRequestHandler<ObrisiPredavacaKomanda, bool>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public ObrisiPredavacaHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<bool> Handle(
            ObrisiPredavacaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var predavac =
                await _jedinicaRada.Predavaci.VratiPoId(zahtev.Id);

            if (predavac is null)
            {
                throw new KeyNotFoundException(
                    $"Predavač sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var sviProgrami =
                await _jedinicaRada.RazvojniProgrami.VratiSve();

            bool predavacVodiProgram = sviProgrami.Any(p =>
                p.PredavacId == zahtev.Id);

            if (predavacVodiProgram)
            {
                throw new InvalidOperationException(
                    "Predavač se ne može obrisati jer je povezan sa jednim ili više razvojnih programa.");
            }

            _jedinicaRada.Predavaci.Obrisi(predavac);
            await _jedinicaRada.SacuvajPromene();

            return true;
        }
    }
}
