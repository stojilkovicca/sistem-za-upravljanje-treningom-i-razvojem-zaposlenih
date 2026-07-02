using MediatR;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Obrada
{
    public class ObradiPrijavuKomanda : IRequest<PrijavaDTO>
    {
        public int ZaposleniId { get; set; }

        public int RazvojniProgramId { get; set; }

        public ObradiPrijavuDTO Podaci { get; set; } = new();
    }
}