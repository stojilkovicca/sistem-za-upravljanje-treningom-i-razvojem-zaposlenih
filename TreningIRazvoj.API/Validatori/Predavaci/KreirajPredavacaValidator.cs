using FluentValidation;
using TreningIRazvoj.API.DTO.Predavaci;

namespace TreningIRazvoj.API.Validatori.Predavaci
{
    public class KreirajPredavacaValidator
        : AbstractValidator<KreirajPredavacaDTO>
    {
        public KreirajPredavacaValidator()
        {
            RuleFor(p => p.Ime)
                .NotEmpty()
                .WithMessage("Ime predavača je obavezno.")
                .MaximumLength(50)
                .WithMessage("Ime predavača može imati najviše 50 karaktera.");

            RuleFor(p => p.Prezime)
                .NotEmpty()
                .WithMessage("Prezime predavača je obavezno.")
                .MaximumLength(50)
                .WithMessage("Prezime predavača može imati najviše 50 karaktera.");

            RuleFor(p => p.Imejl)
                .NotEmpty()
                .WithMessage("Imejl predavača je obavezan.")
                .EmailAddress()
                .WithMessage("Imejl predavača nije u ispravnom formatu.")
                .MaximumLength(150)
                .WithMessage("Imejl predavača može imati najviše 150 karaktera.");

            RuleFor(p => p.OblastStrucnosti)
                .NotEmpty()
                .WithMessage("Oblast stručnosti je obavezna.")
                .MaximumLength(150)
                .WithMessage("Oblast stručnosti može imati najviše 150 karaktera.");
        }
    }
}
