using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiPoId
{
    public class VratiZaposlenogPoIdHandler
        : IRequestHandler<VratiZaposlenogPoIdUpit, ZaposleniDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiZaposlenogPoIdHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<ZaposleniDTO> Handle(
            VratiZaposlenogPoIdUpit zahtev,
            CancellationToken cancellationToken)
        {
            var zaposleni =
                await _jedinicaRada.Zaposleni.VratiPoId(zahtev.Id);

            if (zaposleni is null)
            {
                throw new KeyNotFoundException(
                    $"Zaposleni sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            return new ZaposleniDTO
            {
                Id = zaposleni.Id,
                Ime = zaposleni.Ime,
                Prezime = zaposleni.Prezime,
                Imejl = zaposleni.Imejl,
                RadnoMesto = zaposleni.RadnoMesto,
                Odeljenje = zaposleni.Odeljenje,
                DatumZaposlenja = zaposleni.DatumZaposlenja,
                Aktivan = zaposleni.Aktivan
            };
        }
    }
}
