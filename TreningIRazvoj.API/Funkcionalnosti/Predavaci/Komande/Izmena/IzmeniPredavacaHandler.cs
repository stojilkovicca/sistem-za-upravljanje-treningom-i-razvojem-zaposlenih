using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Izmena
{
    public class IzmeniPredavacaHandler
        : IRequestHandler<IzmeniPredavacaKomanda, PredavacDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public IzmeniPredavacaHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PredavacDTO> Handle(
            IzmeniPredavacaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var predavac =
                await _jedinicaRada.Predavaci.VratiPoId(zahtev.Id);

            if (predavac is null)
            {
                throw new KeyNotFoundException(
                    $"Predavač sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var sviPredavaci =
                await _jedinicaRada.Predavaci.VratiSve();

            bool imejlKoristiDrugiPredavac = sviPredavaci.Any(p =>
                p.Id != zahtev.Id &&
                p.Imejl.Equals(
                    zahtev.Podaci.Imejl,
                    StringComparison.OrdinalIgnoreCase));

            if (imejlKoristiDrugiPredavac)
            {
                throw new InvalidOperationException(
                    "Drugi predavač već koristi unetu imejl adresu.");
            }

            predavac.Ime = zahtev.Podaci.Ime;
            predavac.Prezime = zahtev.Podaci.Prezime;
            predavac.Imejl = zahtev.Podaci.Imejl;
            predavac.OblastStrucnosti =
                zahtev.Podaci.OblastStrucnosti;
            predavac.Interni = zahtev.Podaci.Interni;

            _jedinicaRada.Predavaci.Izmeni(predavac);
            await _jedinicaRada.SacuvajPromene();

            return new PredavacDTO
            {
                Id = predavac.Id,
                Ime = predavac.Ime,
                Prezime = predavac.Prezime,
                Imejl = predavac.Imejl,
                OblastStrucnosti = predavac.OblastStrucnosti,
                Interni = predavac.Interni
            };
        }
    }
}
