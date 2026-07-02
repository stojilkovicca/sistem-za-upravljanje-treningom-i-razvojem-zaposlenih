using MediatR;
using Microsoft.AspNetCore.Identity;
using TreningIRazvoj.Infrastruktura.Identitet;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Registracija
{
    public class RegistrujKorisnikaHandler
        : IRequestHandler<RegistrujKorisnikaKomanda, bool>
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistrujKorisnikaHandler(
            UserManager<Korisnik> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(
            RegistrujKorisnikaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var postojeciKorisnik =
                await _userManager.FindByEmailAsync(
                    zahtev.Podaci.Imejl);

            if (postojeciKorisnik is not null)
            {
                throw new InvalidOperationException(
                    "Korisnik sa unetom imejl adresom već postoji.");
            }

            bool ulogaPostoji =
                await _roleManager.RoleExistsAsync(
                    zahtev.Podaci.Uloga);

            if (!ulogaPostoji)
            {
                var rezultatKreiranjaUloge =
                    await _roleManager.CreateAsync(
                        new IdentityRole(zahtev.Podaci.Uloga));

                if (!rezultatKreiranjaUloge.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Korisnička uloga nije mogla biti kreirana.");
                }
            }

            var korisnik = new Korisnik
            {
                Ime = zahtev.Podaci.Ime,
                Prezime = zahtev.Podaci.Prezime,
                Email = zahtev.Podaci.Imejl,
                UserName = zahtev.Podaci.Imejl
            };

            var rezultatKreiranjaKorisnika =
                await _userManager.CreateAsync(
                    korisnik,
                    zahtev.Podaci.Lozinka);

            if (!rezultatKreiranjaKorisnika.Succeeded)
            {
                string greske = string.Join(
                    " ",
                    rezultatKreiranjaKorisnika.Errors
                        .Select(g => g.Description));

                throw new InvalidOperationException(greske);
            }

            var rezultatDodavanjaUloge =
                await _userManager.AddToRoleAsync(
                    korisnik,
                    zahtev.Podaci.Uloga);

            if (!rezultatDodavanjaUloge.Succeeded)
            {
                await _userManager.DeleteAsync(korisnik);

                throw new InvalidOperationException(
                    "Korisniku nije mogla biti dodeljena uloga.");
            }

            return true;
        }
    }
}