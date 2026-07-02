using FluentValidation;
using TreningIRazvoj.API.Validatori.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Izmena
{
    public class IzmeniPredavacaKomandaValidator
        : AbstractValidator<IzmeniPredavacaKomanda>
    {
        public IzmeniPredavacaKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator predavača mora biti veći od nule.");

            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o predavaču su obavezni.")
                .SetValidator(new IzmeniPredavacaValidator());
        }
    }
}
