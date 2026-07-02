using FluentValidation;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.Pretraga
{
    public class PretraziRazvojneProgrameUpitValidator
        : AbstractValidator<PretraziRazvojneProgrameUpit>
    {
        private static readonly string[] DozvoljenaPoljaZaSortiranje =
        [
            "naziv",
            "datumPocetka",
            "datumZavrsetka",
            "kapacitet",
            "trajanjeUSatima"
        ];

        public PretraziRazvojneProgrameUpitValidator()
        {
            RuleFor(u => u.BrojStranice)
                .GreaterThan(0)
                .WithMessage("Broj stranice mora biti veći od nule.");

            RuleFor(u => u.VelicinaStranice)
                .InclusiveBetween(1, 100)
                .WithMessage(
                    "Veličina stranice mora biti između 1 i 100.");

            RuleFor(u => u.KategorijaProgramaId)
                .GreaterThan(0)
                .When(u => u.KategorijaProgramaId.HasValue)
                .WithMessage(
                    "Identifikator kategorije mora biti veći od nule.");

            RuleFor(u => u.PredavacId)
                .GreaterThan(0)
                .When(u => u.PredavacId.HasValue)
                .WithMessage(
                    "Identifikator predavača mora biti veći od nule.");

            RuleFor(u => u.Vrsta)
                .IsInEnum()
                .When(u => u.Vrsta.HasValue)
                .WithMessage("Vrsta programa nije ispravna.");

            RuleFor(u => u.SortirajPo)
                .Must(vrednost =>
                    DozvoljenaPoljaZaSortiranje.Contains(vrednost))
                .WithMessage(
                    "Sortiranje je dozvoljeno po nazivu, datumu početka, datumu završetka, kapacitetu ili trajanju.");

            RuleFor(u => u.SmerSortiranja)
                .Must(vrednost =>
                    vrednost == "rastuce" ||
                    vrednost == "opadajuce")
                .WithMessage(
                    "Smer sortiranja mora biti rastuce ili opadajuce.");
        }
    }
}
