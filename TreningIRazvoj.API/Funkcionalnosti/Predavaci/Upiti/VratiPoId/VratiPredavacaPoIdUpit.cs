using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiPoId
{
    public class VratiPredavacaPoIdUpit : IRequest<PredavacDTO>
    {
        public int Id { get; set; }
    }
}
