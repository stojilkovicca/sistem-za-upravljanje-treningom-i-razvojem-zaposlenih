using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiSve
{
    public class VratiSvePredavaceHandler
        : IRequestHandler<
            VratiSvePredavaceUpit,
            IEnumerable<PredavacDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiSvePredavaceHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<IEnumerable<PredavacDTO>> Handle(
            VratiSvePredavaceUpit zahtev,
            CancellationToken cancellationToken)
        {
            var predavaci =
                await _jedinicaRada.Predavaci.VratiSve();

            return predavaci.Select(p => new PredavacDTO
            {
                Id = p.Id,
                Ime = p.Ime,
                Prezime = p.Prezime,
                Imejl = p.Imejl,
                OblastStrucnosti = p.OblastStrucnosti,
                Interni = p.Interni
            });
        }
    }
}
