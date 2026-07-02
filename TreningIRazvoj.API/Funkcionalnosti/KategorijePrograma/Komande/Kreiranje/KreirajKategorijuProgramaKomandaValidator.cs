using FluentValidation;
using TreningIRazvoj.API.Validatori.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Kreiranje
{
    public class KreirajKategorijuProgramaKomandaValidator
        : AbstractValidator<KreirajKategorijuProgramaKomanda>
    {
        public KreirajKategorijuProgramaKomandaValidator()
        {
            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage("Podaci o kategoriji programa su obavezni.")
                .SetValidator(new KreirajKategorijuProgramaValidator());
        }
    }
}
