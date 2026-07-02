using FluentValidation;
using TreningIRazvoj.API.Validatori.Predavaci;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Kreiranje
{
    public class KreirajPredavacaKomandaValidator
        : AbstractValidator<KreirajPredavacaKomanda>
    {
        public KreirajPredavacaKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o predavaču su obavezni.")
                .SetValidator(new KreirajPredavacaValidator());
        }
    }
}
