using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Izmena
{
    public class IzmeniRazvojniProgramKomanda
        : IRequest<RazvojniProgramDTO>
    {
        public int Id { get; set; }

        public IzmeniRazvojniProgramDTO Podaci { get; set; } = new();
    }
}
