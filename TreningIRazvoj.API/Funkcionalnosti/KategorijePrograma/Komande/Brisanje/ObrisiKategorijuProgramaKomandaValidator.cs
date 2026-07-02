using FluentValidation;

namespace TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Brisanje
{
    public class ObrisiKategorijuProgramaKomandaValidator
        : AbstractValidator<ObrisiKategorijuProgramaKomanda>
    {
        public ObrisiKategorijuProgramaKomandaValidator()
        {
            RuleFor(k => k.Id)
                .GreaterThan(0)
                .WithMessage(
                    "Identifikator kategorije programa mora biti veći od nule.");
        }
    }
}
