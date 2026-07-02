using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiSve
{
    public class VratiSveKategorijeProgramaHandler
        : IRequestHandler<
            VratiSveKategorijeProgramaUpit,
            IEnumerable<KategorijaProgramaDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiSveKategorijeProgramaHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<IEnumerable<KategorijaProgramaDTO>> Handle(
            VratiSveKategorijeProgramaUpit zahtev,
            CancellationToken cancellationToken)
        {
            var kategorije =
                await _jedinicaRada.KategorijePrograma.VratiSve();

            return kategorije.Select(k => new KategorijaProgramaDTO
            {
                Id = k.Id,
                Naziv = k.Naziv,
                Opis = k.Opis
            });
        }
    }
}
