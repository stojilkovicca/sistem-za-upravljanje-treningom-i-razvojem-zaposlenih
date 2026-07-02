using FluentValidation;
using TreningIRazvoj.API.Validatori.Autentifikacija;

namespace TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Prijavljivanje
{
    public class PrijaviKorisnikaKomandaValidator
        : AbstractValidator<PrijaviKorisnikaKomanda>
    {
        public PrijaviKorisnikaKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci za prijavljivanje su obavezni.")
                .SetValidator(new PrijavaKorisnikaValidator());
        }
    }
}