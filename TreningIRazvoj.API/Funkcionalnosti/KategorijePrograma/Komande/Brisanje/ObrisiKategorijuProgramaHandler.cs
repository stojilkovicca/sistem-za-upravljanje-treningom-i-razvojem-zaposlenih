using MediatR;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Brisanje
{
    public class ObrisiKategorijuProgramaHandler
        : IRequestHandler<ObrisiKategorijuProgramaKomanda, bool>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public ObrisiKategorijuProgramaHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<bool> Handle(
            ObrisiKategorijuProgramaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var kategorija =
                await _jedinicaRada.KategorijePrograma
                    .VratiPoId(zahtev.Id);

            if (kategorija is null)
            {
                throw new KeyNotFoundException(
                    $"Kategorija programa sa identifikatorom {zahtev.Id} nije pronađena.");
            }

            var sviProgrami =
                await _jedinicaRada.RazvojniProgrami.VratiSve();

            bool kategorijaSeKoristi = sviProgrami.Any(p =>
                p.KategorijaProgramaId == zahtev.Id);

            if (kategorijaSeKoristi)
            {
                throw new InvalidOperationException(
                    "Kategorija programa se ne može obrisati jer postoje razvojni programi koji joj pripadaju.");
            }

            _jedinicaRada.KategorijePrograma.Obrisi(kategorija);
            await _jedinicaRada.SacuvajPromene();

            return true;
        }
    }
}
