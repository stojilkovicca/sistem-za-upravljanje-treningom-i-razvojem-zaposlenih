using FluentValidation;
using TreningIRazvoj.API.Validatori.Zaposleni;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Kreiranje
{
    public class KreirajZaposlenogKomandaValidator
        : AbstractValidator<KreirajZaposlenogKomanda>
    {
        public KreirajZaposlenogKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o zaposlenom su obavezni.")
                .SetValidator(new KreirajZaposlenogValidator());
        }
    }
}
