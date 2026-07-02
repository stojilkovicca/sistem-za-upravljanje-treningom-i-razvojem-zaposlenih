using MediatR;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Brisanje
{
    public class ObrisiPredavacaKomanda : IRequest<bool>
    {
        public int Id { get; set; }
    }
}