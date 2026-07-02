using MediatR;
using TreningIRazvoj.API.DTO.Autentifikacija;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Registracija
{
    public class RegistrujKorisnikaKomanda : IRequest<bool>
    {
        public RegistracijaDTO Podaci { get; set; } = new();
    }
}