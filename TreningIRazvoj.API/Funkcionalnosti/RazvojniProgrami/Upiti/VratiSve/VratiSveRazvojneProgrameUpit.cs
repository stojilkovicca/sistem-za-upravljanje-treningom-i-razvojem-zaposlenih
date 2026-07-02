using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiSve
{
    public class VratiSveRazvojneProgrameUpit
        : IRequest<IEnumerable<RazvojniProgramDTO>>
    {
    }
}