using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Kreiranje
{
    public class KreirajRazvojniProgramKomanda
        : IRequest<RazvojniProgramDTO>
    {
        public KreirajRazvojniProgramDTO Podaci { get; set; } = new();
    }
}
