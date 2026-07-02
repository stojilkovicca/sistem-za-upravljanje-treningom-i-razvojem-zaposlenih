using FluentValidation;
using TreningIRazvoj.API.Validatori.KategorijePrograma;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Izmena
{
    public class IzmeniKategorijuProgramaKomandaValidator
        : AbstractValidator<IzmeniKategorijuProgramaKomanda>
    {
        public IzmeniKategorijuProgramaKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator kategorije programa mora biti veći od nule.");

            RuleFor(k => k.Podaci)
                .NotNull()
                .WithMessage(
                    "Podaci o kategoriji programa su obavezni.")
                .SetValidator(
                    new IzmeniKategorijuProgramaValidator());
        }
    }
}
