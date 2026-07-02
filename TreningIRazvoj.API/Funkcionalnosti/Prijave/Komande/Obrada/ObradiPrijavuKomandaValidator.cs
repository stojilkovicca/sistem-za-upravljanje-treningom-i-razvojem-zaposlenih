using FluentValidation;
using TreningIRazvoj.API.Validatori.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Obrada
{
    public class ObradiPrijavuKomandaValidator
        : AbstractValidator<ObradiPrijavuKomanda>
    {
        public ObradiPrijavuKomandaValidator()
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
                .WithMessage("Podaci za obradu prijave su obavezni.")
                .SetValidator(new ObradiPrijavuValidator());
        }
    }
}