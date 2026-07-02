using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Izmena
{
    public class IzmeniZaposlenogKomanda : IRequest<ZaposleniDTO>
    {
        public int Id { get; set; }

        public IzmeniZaposlenogDTO Podaci { get; set; } = new();
    }
}
