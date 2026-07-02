using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiPoId
{
    public class VratiKategorijuProgramaPoIdHandler
        : IRequestHandler<
            VratiKategorijuProgramaPoIdUpit,
            KategorijaProgramaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiKategorijuProgramaPoIdHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<KategorijaProgramaDTO> Handle(
            VratiKategorijuProgramaPoIdUpit zahtev,
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

            return new KategorijaProgramaDTO
            {
                Id = kategorija.Id,
                Naziv = kategorija.Naziv,
                Opis = kategorija.Opis
            };
        }
    }
}
