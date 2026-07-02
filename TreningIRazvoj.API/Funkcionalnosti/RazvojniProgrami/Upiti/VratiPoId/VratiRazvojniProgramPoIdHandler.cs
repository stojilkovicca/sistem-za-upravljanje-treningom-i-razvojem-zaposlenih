using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiPoId
{
    public class VratiRazvojniProgramPoIdHandler
        : IRequestHandler<
            VratiRazvojniProgramPoIdUpit,
            RazvojniProgramDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiRazvojniProgramPoIdHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<RazvojniProgramDTO> Handle(
            VratiRazvojniProgramPoIdUpit zahtev,
            CancellationToken cancellationToken)
        {
            var program =
                await _jedinicaRada.RazvojniProgrami
                    .VratiPoId(zahtev.Id);

            if (program is null)
            {
                throw new KeyNotFoundException(
                    $"Razvojni program sa identifikatorom {zahtev.Id} nije pronađen.");
            }

            var kategorija =
                await _jedinicaRada.KategorijePrograma
                    .VratiPoId(program.KategorijaProgramaId);

            var predavac =
                await _jedinicaRada.Predavaci
                    .VratiPoId(program.PredavacId);

            if (kategorija is null)
            {
                throw new InvalidOperationException(
                    "Kategorija povezana sa razvojnim programom nije pronađena.");
            }

            if (predavac is null)
            {
                throw new InvalidOperationException(
                    "Predavač povezan sa razvojnim programom nije pronađen.");
            }

            return new RazvojniProgramDTO
            {
                Id = program.Id,
                Naziv = program.Naziv,
                Opis = program.Opis,
                Vrsta = program.Vrsta,
                DatumPocetka = program.DatumPocetka,
                DatumZavrsetka = program.DatumZavrsetka,
                Kapacitet = program.Kapacitet,
                TrajanjeUSatima = program.TrajanjeUSatima,
                MinimalanBrojPoena = program.MinimalanBrojPoena,
                Objavljen = program.Objavljen,
                KategorijaProgramaId = program.KategorijaProgramaId,
                NazivKategorije = kategorija.Naziv,
                PredavacId = program.PredavacId,
                ImeIPrezimePredavaca =
                    $"{predavac.Ime} {predavac.Prezime}"
            };
        }
    }
}
