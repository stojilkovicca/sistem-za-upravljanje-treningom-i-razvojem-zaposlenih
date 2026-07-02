using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Izmena
{
    public class IzmeniKategorijuProgramaHandler
        : IRequestHandler<
            IzmeniKategorijuProgramaKomanda,
            KategorijaProgramaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public IzmeniKategorijuProgramaHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<KategorijaProgramaDTO> Handle(
            IzmeniKategorijuProgramaKomanda zahtev,
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

            var sveKategorije =
                await _jedinicaRada.KategorijePrograma.VratiSve();

            bool nazivKoristiDrugaKategorija =
                sveKategorije.Any(k =>
                    k.Id != zahtev.Id &&
                    k.Naziv.Equals(
                        zahtev.Podaci.Naziv,
                        StringComparison.OrdinalIgnoreCase));

            if (nazivKoristiDrugaKategorija)
            {
                throw new InvalidOperationException(
                    "Druga kategorija programa već koristi uneti naziv.");
            }

            kategorija.Naziv = zahtev.Podaci.Naziv;
            kategorija.Opis = zahtev.Podaci.Opis;

            _jedinicaRada.KategorijePrograma.Izmeni(kategorija);
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
