using FluentValidation;
using TreningIRazvoj.API.DTO.Zaposleni;

namespace TreningIRazvoj.API.Validatori.Zaposleni
{
    public class KreirajZaposlenogValidator
        : AbstractValidator<KreirajZaposlenogDTO>
    {
        public KreirajZaposlenogValidator()
        {
            RuleFor(z => z.Ime)
                .NotEmpty()
                .WithMessage("Ime je obavezno.")
                .MaximumLength(50)
                .WithMessage("Ime može imati najviše 50 karaktera.");

            RuleFor(z => z.Prezime)
                .NotEmpty()
                .WithMessage("Prezime je obavezno.")
                .MaximumLength(50)
                .WithMessage("Prezime može imati najviše 50 karaktera.");

            RuleFor(z => z.Imejl)
                .NotEmpty()
                .WithMessage("Imejl je obavezan.")
                .EmailAddress()
                .WithMessage("Imejl nije u ispravnom formatu.")
                .MaximumLength(150)
                .WithMessage("Imejl može imati najviše 150 karaktera.");

            RuleFor(z => z.RadnoMesto)
                .NotEmpty()
                .WithMessage("Radno mesto je obavezno.")
                .MaximumLength(100)
                .WithMessage("Radno mesto može imati najviše 100 karaktera.");

            RuleFor(z => z.Odeljenje)
                .NotEmpty()
                .WithMessage("Odeljenje je obavezno.")
                .MaximumLength(100)
                .WithMessage("Odeljenje može imati najviše 100 karaktera.");

            RuleFor(z => z.DatumZaposlenja)
                .NotEmpty()
                .WithMessage("Datum zaposlenja je obavezan.")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Datum zaposlenja ne može biti u budućnosti.");
        }
    }
}
