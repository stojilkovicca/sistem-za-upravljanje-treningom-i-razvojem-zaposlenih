using MediatR;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Ocenjivanje
{
    public class OceniProgramKomanda : IRequest<PrijavaDTO>
    {
        public int ZaposleniId { get; set; }

        public int RazvojniProgramId { get; set; }

        public OceniProgramDTO Podaci { get; set; } = new();
    }
}