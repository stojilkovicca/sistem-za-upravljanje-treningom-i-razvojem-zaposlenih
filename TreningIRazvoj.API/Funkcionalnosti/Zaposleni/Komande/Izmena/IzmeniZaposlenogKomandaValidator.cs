using FluentValidation;
using TreningIRazvoj.API.Validatori.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Izmena
{
    public class IzmeniZaposlenogKomandaValidator
        : AbstractValidator<IzmeniZaposlenogKomanda>
    {
        public IzmeniZaposlenogKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage("Identifikator zaposlenog mora biti veći od nule.");

            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o zaposlenom su obavezni.")
                .SetValidator(new IzmeniZaposlenogValidator());
        }
    }
}
