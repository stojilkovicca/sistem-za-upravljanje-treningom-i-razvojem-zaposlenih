using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiPoId
{
    public class VratiPredavacaPoIdHandler
        : IRequestHandler<VratiPredavacaPoIdUpit, PredavacDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiPredavacaPoIdHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PredavacDTO> Handle(
            VratiPredavacaPoIdUpit zahtev,
            CancellationToken cancellationToken)
        {
            var predavac =
                await _jedinicaRada.Predavaci.VratiPoId(zahtev.Id);

            if (predavac is null)
            {
                throw new KeyNotFoundException(
                    $"Predavač sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            return new PredavacDTO
            {
                Id = predavac.Id,
                Ime = predavac.Ime,
                Prezime = predavac.Prezime,
                Imejl = predavac.Imejl,
                OblastStrucnosti = predavac.OblastStrucnosti,
                Interni = predavac.Interni
            };
        }
    }
}
