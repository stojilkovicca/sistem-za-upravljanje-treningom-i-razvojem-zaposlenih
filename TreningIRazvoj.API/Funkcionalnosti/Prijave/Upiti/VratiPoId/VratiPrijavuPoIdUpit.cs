using MediatR;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiPoId
{
    public class VratiPrijavuPoIdUpit : IRequest<PrijavaDTO>
    {
        public int ZaposleniId { get; set; }

        public int RazvojniProgramId { get; set; }
    }
}