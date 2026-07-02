using FluentValidation;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Brisanje
{
    public class ObrisiZaposlenogKomandaValidator
        : AbstractValidator<ObrisiZaposlenogKomanda>
    {
        public ObrisiZaposlenogKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator zaposlenog mora biti veći od nule.");
        }
    }
}
