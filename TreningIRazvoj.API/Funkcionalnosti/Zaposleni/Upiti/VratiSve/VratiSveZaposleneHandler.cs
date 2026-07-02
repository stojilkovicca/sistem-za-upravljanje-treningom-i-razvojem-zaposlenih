using MediatR;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiSve
{
    public class VratiSveZaposleneHandler
        : IRequestHandler<
            VratiSveZaposleneUpit,
            IEnumerable<ZaposleniDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiSveZaposleneHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<IEnumerable<ZaposleniDTO>> Handle(
            VratiSveZaposleneUpit zahtev,
            CancellationToken cancellationToken)
        {
            var zaposleni = await _jedinicaRada.Zaposleni.VratiSve();

            return zaposleni.Select(z => new ZaposleniDTO
            {
                Id = z.Id,
                Ime = z.Ime,
                Prezime = z.Prezime,
                Imejl = z.Imejl,
                RadnoMesto = z.RadnoMesto,
                Odeljenje = z.Odeljenje,
                DatumZaposlenja = z.DatumZaposlenja,
                Aktivan = z.Aktivan
            });
        }
    }
}
