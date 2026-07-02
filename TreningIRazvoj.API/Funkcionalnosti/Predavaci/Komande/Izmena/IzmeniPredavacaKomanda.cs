using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Izmena
{
    public class IzmeniPredavacaKomanda : IRequest<PredavacDTO>
    {
        public int Id { get; set; }

        public IzmeniPredavacaDTO Podaci { get; set; } = new();
    }
}
