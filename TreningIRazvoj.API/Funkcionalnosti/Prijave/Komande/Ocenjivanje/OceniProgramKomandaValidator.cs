using FluentValidation;
using TreningIRazvoj.API.Validatori.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Ocenjivanje
{
    public class OceniProgramKomandaValidator
        : AbstractValidator<OceniProgramKomanda>
    {
        public OceniProgramKomandaValidator()
        {
            RuleFor(k => k.ZaposleniId)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator zaposlenog mora biti veći od nule.");

            RuleFor(k => k.RazvojniProgramId)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator razvojnog programa mora biti veći od nule.");

            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o oceni programa su obavezni.")
                .SetValidator(new OceniProgramValidator());
        }
    }
}