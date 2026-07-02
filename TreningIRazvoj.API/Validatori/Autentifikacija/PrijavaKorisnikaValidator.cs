using FluentValidation;
using TreningIRazvoj.API.DTO.Autentifikacija;

namespace TreningIRazvoj.API.Validatori.Autentifikacija
{
    public class PrijavaKorisnikaValidator
        : AbstractValidator<PrijavaKorisnikaDTO>
    {
        public PrijavaKorisnikaValidator()
        {
            RuleFor(p => p.Imejl)
                .NotEmpty()
                .WithMessage("Imejl je obavezan.")
                .EmailAddress()
                .WithMessage("Imejl nije u ispravnom formatu.");

            RuleFor(p => p.Lozinka)
                .NotEmpty()
                .WithMessage("Lozinka je obavezna.");
        }
    }
}