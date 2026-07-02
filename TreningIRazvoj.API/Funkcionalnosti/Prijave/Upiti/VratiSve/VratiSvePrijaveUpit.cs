using MediatR;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiSve
{
    public class VratiSvePrijaveUpit
        : IRequest<IEnumerable<PrijavaDTO>>
    {
    }
}
