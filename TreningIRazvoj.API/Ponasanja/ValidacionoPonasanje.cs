using FluentValidation;
using MediatR;

namespace TreningIRazvoj.API.Ponasanja
{
    public class ValidacionoPonasanje<TZahtev, TOdgovor>
        : IPipelineBehavior<TZahtev, TOdgovor>
        where TZahtev : notnull
    {
        private readonly IEnumerable<IValidator<TZahtev>> _validatori;

        public ValidacionoPonasanje(
            IEnumerable<IValidator<TZahtev>> validatori)
        {
            _validatori = validatori;
        }

        public async Task<TOdgovor> Handle(
            TZahtev zahtev,
            RequestHandlerDelegate<TOdgovor> sledeci,
            CancellationToken cancellationToken)
        {
            if (!_validatori.Any())
            {
                return await sledeci(cancellationToken);
            }

            var kontekst = new ValidationContext<TZahtev>(zahtev);

            var rezultati = await Task.WhenAll(
                _validatori.Select(validator =>
                    validator.ValidateAsync(kontekst, cancellationToken)));

            var greske = rezultati
                .SelectMany(rezultat => rezultat.Errors)
                .Where(greska => greska != null)
                .ToList();

            if (greske.Count != 0)
            {
                throw new ValidationException(greske);
            }

            return await sledeci(cancellationToken);
        }
    }
}
