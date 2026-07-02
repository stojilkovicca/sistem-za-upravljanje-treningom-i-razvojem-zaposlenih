using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiPoId
{
    public class VratiZaposlenogPoIdUpit : IRequest<ZaposleniDTO>
    {
        public int Id { get; set; }
    }
}
