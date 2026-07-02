using MediatR;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Brisanje
{
    public class ObrisiZaposlenogKomanda : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
