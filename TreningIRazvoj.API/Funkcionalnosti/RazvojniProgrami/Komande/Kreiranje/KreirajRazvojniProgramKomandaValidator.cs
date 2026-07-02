using FluentValidation;
using TreningIRazvoj.API.Validatori.RazvojniProgrami;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Kreiranje
{
    public class KreirajRazvojniProgramKomandaValidator
        : AbstractValidator<KreirajRazvojniProgramKomanda>
    {
        public KreirajRazvojniProgramKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o razvojnom programu su obavezni.")
                .SetValidator(new KreirajRazvojniProgramValidator());
        }
    }
}
