using FluentValidation;
using TreningIRazvoj.API.DTO.KategorijePrograma;

namespace TreningIRazvoj.API.Validatori.KategorijePrograma
{
    public class IzmeniKategorijuProgramaValidator
        : AbstractValidator<IzmeniKategorijuProgramaDTO>
    {
        public IzmeniKategorijuProgramaValidator()
        {
            RuleFor(k => k.Naziv)
                .NotEmpty()
                .WithMessage("Naziv kategorije je obavezan.")
                .MaximumLength(100)
                .WithMessage("Naziv kategorije može imati najviše 100 karaktera.");

            RuleFor(k => k.Opis)
                .NotEmpty()
                .WithMessage("Opis kategorije je obavezan.")
                .MaximumLength(500)
                .WithMessage("Opis kategorije može imati najviše 500 karaktera.");
        }
    }
}