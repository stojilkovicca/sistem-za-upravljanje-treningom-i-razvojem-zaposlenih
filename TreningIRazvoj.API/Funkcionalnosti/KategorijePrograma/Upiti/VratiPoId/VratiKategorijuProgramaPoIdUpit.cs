using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiPoId
{
    public class VratiKategorijuProgramaPoIdUpit
        : IRequest<KategorijaProgramaDTO>
    {
        public int Id { get; set; }
    }
}