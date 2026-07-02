using FluentValidation;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Brisanje
{
    public class ObrisiPredavacaKomandaValidator
        : AbstractValidator<ObrisiPredavacaKomanda>
    {
        public ObrisiPredavacaKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator predavača mora biti veći od nule.");
        }
    }
}
