using FluentValidation;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Brisanje
{
    public class ObrisiRazvojniProgramKomandaValidator
        : AbstractValidator<ObrisiRazvojniProgramKomanda>
    {
        public ObrisiRazvojniProgramKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator razvojnog programa mora biti veći od nule.");
        }
    }
}