using MediatR;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiPoId
{
    public class VratiPrijavuPoIdHandler
        : IRequestHandler<VratiPrijavuPoIdUpit, PrijavaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiPrijavuPoIdHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PrijavaDTO> Handle(
            VratiPrijavuPoIdUpit zahtev,
            CancellationToken cancellationToken)
        {
            var prijava =
                await _jedinicaRada.Prijave.VratiPoId(
                    zahtev.ZaposleniId,
                    zahtev.RazvojniProgramId);

            if (prijava is null)
            {
                throw new KeyNotFoundException(
                    $"Prijava zaposlenog {zahtev.ZaposleniId} za razvojni program {zahtev.RazvojniProgramId} nije pronađena.");
            }

            var zaposleni =
                await _jedinicaRada.Zaposleni
                    .VratiPoId(prijava.ZaposleniId);

            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(prijava.RazvojniProgramId);

            if (zaposleni is null)
            {
                throw new InvalidOperationException(
                    "Zaposleni povezan sa prijavom nije pronađen.");
            }

            if (program is null)
            {
                throw new InvalidOperationException(
                    "Razvojni program povezan sa prijavom nije pronađen.");
            }

            return new PrijavaDTO
            {
                ZaposleniId = prijava.ZaposleniId,
                ImeIPrezimeZaposlenog =
                    $"{zaposleni.Ime} {zaposleni.Prezime}",

                RazvojniProgramId = prijava.RazvojniProgramId,
                NazivRazvojnogPrograma = program.Naziv,

                DatumPrijave = prijava.DatumPrijave,
                Status = prijava.Status,
                ProcenatPrisustva = prijava.ProcenatPrisustva,
                BrojPoena = prijava.BrojPoena,
                DatumZavrsetka = prijava.DatumZavrsetka,
                OcenaPrograma = prijava.OcenaPrograma
            };
        }
    }
}