using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiPoId
{
    public class VratiRazvojniProgramPoIdUpit
        : IRequest<RazvojniProgramDTO>
    {
        public int Id { get; set; }
    }
}