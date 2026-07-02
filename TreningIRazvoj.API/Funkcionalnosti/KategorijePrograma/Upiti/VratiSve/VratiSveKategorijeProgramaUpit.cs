using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiSve
{
    public class VratiSveKategorijeProgramaUpit
        : IRequest<IEnumerable<KategorijaProgramaDTO>>
    {
    }
}
