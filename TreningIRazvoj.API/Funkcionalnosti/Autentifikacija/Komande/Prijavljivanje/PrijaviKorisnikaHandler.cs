using MediatR;
using Microsoft.AspNetCore.Identity;
using TreningIRazvoj.API.DTO.Autentifikacija;
using TreningIRazvoj.API.Servisi;
using TreningIRazvoj.Infrastruktura.Identitet;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Prijavljivanje
{
    public class PrijaviKorisnikaHandler
        : IRequestHandler<PrijaviKorisnikaKomanda, TokenDTO>
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly IJwtServis _jwtServis;

        public PrijaviKorisnikaHandler(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            IJwtServis jwtServis)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtServis = jwtServis;
        }

        public async Task<TokenDTO> Handle(
            PrijaviKorisnikaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var korisnik = await _userManager.FindByEmailAsync(
                zahtev.Podaci.Imejl);

            if (korisnik is null)
            {
                throw new UnauthorizedAccessException(
                    "Imejl ili lozinka nisu ispravni.");
            }

            var rezultatPrijave =
                await _signInManager.CheckPasswordSignInAsync(
                    korisnik,
                    zahtev.Podaci.Lozinka,
                    lockoutOnFailure: false);

            if (!rezultatPrijave.Succeeded)
            {
                throw new UnauthorizedAccessException(
                    "Imejl ili lozinka nisu ispravni.");
            }

            return await _jwtServis.GenerisiToken(korisnik);
        }
    }
}