using TreningIRazvoj.API.DTO.Autentifikacija;
using TreningIRazvoj.Infrastruktura.Identitet;

namespace TreningIRazvoj.API.Servisi
{
    public interface IJwtServis
    {
        Task<TokenDTO> GenerisiToken(Korisnik korisnik);
    }
}