using MediatR;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Brisanje
{
    public class ObrisiRazvojniProgramKomanda : IRequest<bool>
    {
        public int Id { get; set; }
    }
}