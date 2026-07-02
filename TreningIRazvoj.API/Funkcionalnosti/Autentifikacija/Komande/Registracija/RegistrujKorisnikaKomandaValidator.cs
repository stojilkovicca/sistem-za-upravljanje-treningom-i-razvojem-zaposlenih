using FluentValidation;
using TreningIRazvoj.API.Validatori.Autentifikacija;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Registracija
{
    public class RegistrujKorisnikaKomandaValidator
        : AbstractValidator<RegistrujKorisnikaKomanda>
    {
        public RegistrujKorisnikaKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci za registraciju su obavezni.")
                .SetValidator(new RegistracijaValidator());
        }
    }
}