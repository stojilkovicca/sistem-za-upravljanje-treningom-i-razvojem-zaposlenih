using MediatR;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiSve
{
    public class VratiSvePrijaveHandler
        : IRequestHandler<
            VratiSvePrijaveUpit,
            IEnumerable<PrijavaDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiSvePrijaveHandler(IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<IEnumerable<PrijavaDTO>> Handle(
            VratiSvePrijaveUpit zahtev,
            CancellationToken cancellationToken)
        {
            var prijave =
                await _jedinicaRada.Prijave.VratiSve();

            var zaposleni =
                await _jedinicaRada.Zaposleni.VratiSve();

            var programi =
                await _jedinicaRada.RazvojniProgrami.VratiSve();

            return prijave.Select(prijava =>
            {
                var zaposleniZaPrijavu = zaposleni.First(
                    z => z.Id == prijava.ZaposleniId);

                var programZaPrijavu = programi.First(
                    p => p.Id == prijava.RazvojniProgramId);

                return new PrijavaDTO
                {
                    ZaposleniId = prijava.ZaposleniId,
                    ImeIPrezimeZaposlenog =
                        $"{zaposleniZaPrijavu.Ime} {zaposleniZaPrijavu.Prezime}",

                    RazvojniProgramId = prijava.RazvojniProgramId,
                    NazivRazvojnogPrograma = programZaPrijavu.Naziv,

                    DatumPrijave = prijava.DatumPrijave,
                    Status = prijava.Status,
                    ProcenatPrisustva = prijava.ProcenatPrisustva,
                    BrojPoena = prijava.BrojPoena,
                    DatumZavrsetka = prijava.DatumZavrsetka,
                    OcenaPrograma = prijava.OcenaPrograma
                };
            });
        }
    }
}