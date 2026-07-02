using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Kreiranje
{
    public class KreirajZaposlenogKomanda : IRequest<ZaposleniDTO>
    {
        public KreirajZaposlenogDTO Podaci { get; set; } = new();
    }
}
