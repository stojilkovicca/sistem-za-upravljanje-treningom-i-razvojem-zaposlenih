using FluentValidation;
using TreningIRazvoj.API.Validatori.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Izmena
{
    public class IzmeniRazvojniProgramKomandaValidator
        : AbstractValidator<IzmeniRazvojniProgramKomanda>
    {
        public IzmeniRazvojniProgramKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator razvojnog programa mora biti veći od nule.");

            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage(
                    "Podaci o razvojnom programu su obavezni.")
                .SetValidator(
                    new IzmeniRazvojniProgramValidator());
        }
    }
}
