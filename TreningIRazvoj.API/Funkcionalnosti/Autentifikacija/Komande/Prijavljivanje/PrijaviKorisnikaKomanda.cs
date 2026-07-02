using MediatR;
using TreningIRazvoj.API.DTO.Autentifikacija;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Prijavljivanje
{
    public class PrijaviKorisnikaKomanda : IRequest<TokenDTO>
    {
        public PrijavaKorisnikaDTO Podaci { get; set; } = new();
    }
}