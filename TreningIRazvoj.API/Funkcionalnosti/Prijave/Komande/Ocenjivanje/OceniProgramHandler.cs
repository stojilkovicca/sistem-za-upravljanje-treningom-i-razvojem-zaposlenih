using MediatR;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.Domen.Enumeracije;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Ocenjivanje
{
    public class OceniProgramHandler
        : IRequestHandler<OceniProgramKomanda, PrijavaDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public OceniProgramHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<PrijavaDTO> Handle(
            OceniProgramKomanda zahtev,
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

            if (prijava.Status != StatusPrijave.Zavrsena)
            {
                throw new InvalidOperationException(
                    "Razvojni program može biti ocenjen samo nakon završetka.");
            }

            if (prijava.OcenaPrograma.HasValue)
            {
                throw new InvalidOperationException(
                    "Razvojni program je već ocenjen.");
            }

            var zaposleni =
                await _jedinicaRada.Zaposleni
                    .VratiPoId(zahtev.ZaposleniId);

            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(zahtev.RazvojniProgramId);

            if (zaposleni is null || program is null)
            {
                throw new InvalidOperationException(
                    "Podaci povezani sa prijavom nisu pronađeni.");
            }

            prijava.OcenaPrograma = zahtev.Podaci.OcenaPrograma;

            _jedinicaRada.Prijave.Izmeni(prijava);
            await _jedinicaRada.SacuvajPromene();

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