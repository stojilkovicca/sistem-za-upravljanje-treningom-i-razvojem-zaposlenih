using MediatR;
using TreningIRazvoj.API.DTO.Predavaci;
using TreningIRazvoj.Domen.Interfejsi;
using PredavacEntitet = TreningIRazvoj.Domen.Modeli.Predavac;

namespace TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Kreiranje
{
    public class KreirajPredavacaHandler
        : IRequestHandler<KreirajPredavacaKomanda, PredavacDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public KreirajPredavacaHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PredavacDTO> Handle(
            KreirajPredavacaKomanda zahtev,
            CancellationToken cancellationToken)
        {
            var sviPredavaci =
                await _jedinicaRada.Predavaci.VratiSve();

            bool imejlPostoji = sviPredavaci.Any(p =>
                p.Imejl.Equals(
                    zahtev.Podaci.Imejl,
                    StringComparison.OrdinalIgnoreCase));

            if (imejlPostoji)
            {
                throw new InvalidOperationException(
                    "Predavač sa unetom imejl adresom već postoji.");
            }

            var predavac = new PredavacEntitet
            {
                Ime = zahtev.Podaci.Ime,
                Prezime = zahtev.Podaci.Prezime,
                Imejl = zahtev.Podaci.Imejl,
                OblastStrucnosti = zahtev.Podaci.OblastStrucnosti,
                Interni = zahtev.Podaci.Interni
            };

            await _jedinicaRada.Predavaci.Dodaj(predavac);
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