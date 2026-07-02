using FluentValidation;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Validatori.Prijave
{
    public class KreirajPrijavuValidator
        : AbstractValidator<KreirajPrijavuDTO>
    {
        public KreirajPrijavuValidator()
        {
            RuleFor(p => p.ZaposleniId)
                .GreaterThan(0)
                .WithMessage("Identifikator zaposlenog mora biti veći od nule.");

            RuleFor(p => p.RazvojniProgramId)
                .GreaterThan(0)
                .WithMessage("Identifikator razvojnog programa mora biti veći od nule.");
        }
    }
}
