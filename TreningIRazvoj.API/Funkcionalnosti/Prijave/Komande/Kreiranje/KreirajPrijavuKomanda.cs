using MediatR;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Kreiranje
{
    public class KreirajPrijavuKomanda : IRequest<PrijavaDTO>
    {
        public KreirajPrijavuDTO Podaci { get; set; } = new();
    }
}