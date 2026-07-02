using FluentValidation;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Validatori.Prijave
{
    public class OceniProgramValidator
        : AbstractValidator<OceniProgramDTO>
    {
        public OceniProgramValidator()
        {
            RuleFor(p => p.OcenaPrograma)
                .InclusiveBetween(1, 5)
                .WithMessage("Ocena programa mora biti između 1 i 5.");
        }
    }
}
