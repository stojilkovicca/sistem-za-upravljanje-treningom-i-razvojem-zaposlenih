using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiSve
{
    public class VratiSvePredavaceUpit
        : IRequest<IEnumerable<PredavacDTO>>
    {
    }
}
