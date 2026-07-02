using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiSve
{
    public class VratiSveZaposleneUpit
        : IRequest<IEnumerable<ZaposleniDTO>>
    {
    }
}
