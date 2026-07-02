using FluentValidation;
using TreningIRazvoj.API.DTO.RazvojniProgrami;

namespace TreningIRazvoj.API.Validatori.RazvojniProgrami
{
    public class IzmeniRazvojniProgramValidator
        : AbstractValidator<IzmeniRazvojniProgramDTO>
    {
        public IzmeniRazvojniProgramValidator()
        {
            RuleFor(r => r.Naziv)
                .NotEmpty()
                .WithMessage("Naziv razvojnog programa je obavezan.")
                .MaximumLength(150)
                .WithMessage("Naziv razvojnog programa može imati najviše 150 karaktera.");

            RuleFor(r => r.Opis)
                .NotEmpty()
                .WithMessage("Opis razvojnog programa je obavezan.")
                .MaximumLength(1000)
                .WithMessage("Opis razvojnog programa može imati najviše 1000 karaktera.");

            RuleFor(r => r.Vrsta)
                .IsInEnum()
                .WithMessage("Vrsta programa nije ispravna.");

            RuleFor(r => r.DatumPocetka)
                .NotEmpty()
                .WithMessage("Datum početka je obavezan.");

            RuleFor(r => r.DatumZavrsetka)
                .NotEmpty()
                .WithMessage("Datum završetka je obavezan.")
                .GreaterThan(r => r.DatumPocetka)
                .WithMessage("Datum završetka mora biti posle datuma početka.");

            RuleFor(r => r.Kapacitet)
                .GreaterThan(0)
                .WithMessage("Kapacitet mora biti veći od nule.");

            RuleFor(r => r.TrajanjeUSatima)
                .GreaterThan(0)
                .WithMessage("Trajanje programa mora biti veće od nula sati.");

            RuleFor(r => r.MinimalanBrojPoena)
                .InclusiveBetween(0, 100)
                .When(r => r.MinimalanBrojPoena.HasValue)
                .WithMessage("Minimalan broj poena mora biti između 0 i 100.");

            RuleFor(r => r.KategorijaProgramaId)
                .GreaterThan(0)
                .WithMessage("Kategorija programa je obavezna.");

            RuleFor(r => r.PredavacId)
                .GreaterThan(0)
                .WithMessage("Predavač je obavezan.");
        }
    }
}
