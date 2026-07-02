using FluentValidation;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Validatori.Prijave
{
    public class ObradiPrijavuValidator
        : AbstractValidator<ObradiPrijavuDTO>
    {
        public ObradiPrijavuValidator()
        {
            RuleFor(p => p.Status)
                .IsInEnum()
                .WithMessage("Status prijave nije ispravan.");

            RuleFor(p => p.ProcenatPrisustva)
                .InclusiveBetween(0, 100)
                .When(p => p.ProcenatPrisustva.HasValue)
                .WithMessage("Procenat prisustva mora biti između 0 i 100.");

            RuleFor(p => p.BrojPoena)
                .InclusiveBetween(0, 100)
                .When(p => p.BrojPoena.HasValue)
                .WithMessage("Broj poena mora biti između 0 i 100.");

            RuleFor(p => p.DatumZavrsetka)
                .LessThanOrEqualTo(DateTime.Today)
                .When(p => p.DatumZavrsetka.HasValue)
                .WithMessage("Datum završetka ne može biti u budućnosti.");
        }
    }
}
