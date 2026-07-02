using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TreningIRazvoj.API.DTO.Autentifikacija;
using TreningIRazvoj.API.Podesavanja;
using TreningIRazvoj.Infrastruktura.Identitet;

namespace TreningIRazvoj.API.Servisi
{
    public class JwtServis : IJwtServis
    {
        private readonly JwtPodesavanja _jwtPodesavanja;
        private readonly UserManager<Korisnik> _userManager;

        public JwtServis(
            IOptions<JwtPodesavanja> jwtPodesavanja,
            UserManager<Korisnik> userManager)
        {
            _jwtPodesavanja = jwtPodesavanja.Value;
            _userManager = userManager;
        }

        public async Task<TokenDTO> GenerisiToken(Korisnik korisnik)
        {
            var uloge = await _userManager.GetRolesAsync(korisnik);

            var tvrdnje = new List<Claim>
            {
                new(
                    JwtRegisteredClaimNames.Sub,
                    korisnik.Id),

                new(
                    JwtRegisteredClaimNames.Email,
                    korisnik.Email ?? string.Empty),

                new(
                    ClaimTypes.NameIdentifier,
                    korisnik.Id),

                new(
                    ClaimTypes.Name,
                    korisnik.UserName ?? string.Empty),

                new(
                    "ime",
                    korisnik.Ime),

                new(
                    "prezime",
                    korisnik.Prezime),

                new(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            foreach (var uloga in uloge)
            {
                tvrdnje.Add(new Claim(ClaimTypes.Role, uloga));
            }

            var kljuc = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtPodesavanja.Kljuc));

            var podaciZaPotpis = new SigningCredentials(
                kljuc,
                SecurityAlgorithms.HmacSha256);

            var vremeIsteka = DateTime.UtcNow.AddMinutes(
                _jwtPodesavanja.TrajanjeUMinutima);

            var token = new JwtSecurityToken(
                issuer: _jwtPodesavanja.Izdavalac,
                audience: _jwtPodesavanja.Primalac,
                claims: tvrdnje,
                expires: vremeIsteka,
                signingCredentials: podaciZaPotpis);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler()
                    .WriteToken(token),

                Istice = vremeIsteka,
                Imejl = korisnik.Email ?? string.Empty,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Uloge = uloge
            };
        }
    }
}