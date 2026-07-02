using MediatR;
using TreningIRazvoj.API.DTO.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Kreiranje
{
    public class KreirajKategorijuProgramaKomanda
        : IRequest<KategorijaProgramaDTO>
    {
        public KreirajKategorijuProgramaDTO Podaci { get; set; } = new();
    }
}
