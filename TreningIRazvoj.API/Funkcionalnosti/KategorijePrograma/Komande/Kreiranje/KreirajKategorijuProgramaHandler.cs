using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;
using TreningIRazvoj.Domen.Interfejsi;
using KategorijaEntitet =
    TreningIRazvoj.Domen.Modeli.KategorijaPrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Kreiranje
{
    public class KreirajKategorijuProgramaHandler
        : IRequestHandler<
            KreirajKategorijuProgramaKomanda,
            KategorijaProgramaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public KreirajKategorijuProgramaHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<KategorijaProgramaDTO> Handle(
            KreirajKategorijuProgramaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var sveKategorije =
                await _jedinicaRada.KategorijePrograma.VratiSve();

            bool nazivPostoji = sveKategorije.Any(k =>
                k.Naziv.Equals(
                    zahtev.Podaci.Naziv,
                    StringComparison.OrdinalIgnoreCase));

            if (nazivPostoji)
            {
                throw new InvalidOperationException(
                    "Kategorija programa sa unetim nazivom već postoji.");
            }

            var kategorija = new KategorijaEntitet
            {
                Naziv = zahtev.Podaci.Naziv,
                Opis = zahtev.Podaci.Opis
            };

            await _jedinicaRada.KategorijePrograma.Dodaj(kategorija);
            await _jedinicaRada.SacuvajPromene();

            return new KategorijaProgramaDTO
            {
                Id = kategorija.Id,
                Naziv = kategorija.Naziv,
                Opis = kategorija.Opis
            };
        }
    }
}
