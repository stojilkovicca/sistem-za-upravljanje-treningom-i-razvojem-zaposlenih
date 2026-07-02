using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Izmena
{
    public class IzmeniKategorijuProgramaKomanda
        : IRequest<KategorijaProgramaDTO>
    {
        public int Id { get; set; }

        public IzmeniKategorijuProgramaDTO Podaci { get; set; } = new();
    }
}
