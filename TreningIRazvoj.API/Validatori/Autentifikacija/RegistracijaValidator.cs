using FluentValidation;
using TreningIRazvoj.API.DTO.Autentifikacija;

namespace TreningIRazvoj.API.Validatori.Autentifikacija
{
    public class RegistracijaValidator
        : AbstractValidator<RegistracijaDTO>
    {
        private static readonly string[] DozvoljeneUloge =
        [
            "Administrator",
            "HR",
            "Zaposleni"
        ];

        public RegistracijaValidator()
        {
            RuleFor(r => r.Ime)
                .NotEmpty()
                .WithMessage("Ime je obavezno.")
                .MaximumLength(50)
                .WithMessage("Ime može imati najviše 50 karaktera.");

            RuleFor(r => r.Prezime)
                .NotEmpty()
                .WithMessage("Prezime je obavezno.")
                .MaximumLength(50)
                .WithMessage("Prezime može imati najviše 50 karaktera.");

            RuleFor(r => r.Imejl)
                .NotEmpty()
                .WithMessage("Imejl je obavezan.")
                .EmailAddress()
                .WithMessage("Imejl nije u ispravnom formatu.")
                .MaximumLength(150)
                .WithMessage("Imejl može imati najviše 150 karaktera.");

            RuleFor(r => r.Lozinka)
                .NotEmpty()
                .WithMessage("Lozinka je obavezna.")
                .MinimumLength(8)
                .WithMessage("Lozinka mora imati najmanje 8 karaktera.");

            RuleFor(r => r.Uloga)
                .NotEmpty()
                .WithMessage("Uloga je obavezna.")
                .Must(uloga => DozvoljeneUloge.Contains(uloga))
                .WithMessage(
                    "Uloga mora biti Administrator, HR ili Zaposleni.");
        }
    }
}