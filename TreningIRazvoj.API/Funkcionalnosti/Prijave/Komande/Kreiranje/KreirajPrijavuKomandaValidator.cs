using FluentValidation;
using TreningIRazvoj.API.Validatori.Prijave;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Kreiranje
{
    public class KreirajPrijavuKomandaValidator
        : AbstractValidator<KreirajPrijavuKomanda>
    {
        public KreirajPrijavuKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o prijavi su obavezni.")
                .SetValidator(new KreirajPrijavuValidator());
        }
    }
}