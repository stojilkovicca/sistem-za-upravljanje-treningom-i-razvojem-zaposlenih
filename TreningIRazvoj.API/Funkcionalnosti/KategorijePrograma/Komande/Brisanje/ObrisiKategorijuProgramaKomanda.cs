using MediatR;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Brisanje
{
    public class ObrisiKategorijuProgramaKomanda : IRequest<bool>
    {
        public int Id { get; set; }
    }
}