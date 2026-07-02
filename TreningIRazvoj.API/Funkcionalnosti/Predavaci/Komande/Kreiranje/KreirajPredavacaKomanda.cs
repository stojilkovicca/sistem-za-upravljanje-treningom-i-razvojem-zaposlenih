using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Kreiranje
{
    public class KreirajPredavacaKomanda : IRequest<PredavacDTO>
    {
        public KreirajPredavacaDTO Podaci { get; set; } = new();
    }
}
