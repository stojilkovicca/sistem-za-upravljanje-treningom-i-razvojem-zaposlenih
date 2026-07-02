using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiSve
{
    public class VratiSveRazvojneProgrameHandler
        : IRequestHandler<
            VratiSveRazvojneProgrameUpit,
            IEnumerable<RazvojniProgramDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public VratiSveRazvojneProgrameHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<IEnumerable<RazvojniProgramDTO>> Handle(
            VratiSveRazvojneProgrameUpit zahtev,
            CancellationToken cancellationToken)
        {
            var programi =
                await _jedinicaRada.RazvojniProgrami.VratiSve();

            var kategorije =
                await _jedinicaRada.KategorijePrograma.VratiSve();

            var predavaci =
                await _jedinicaRada.Predavaci.VratiSve();

            return programi.Select(program =>
            {
                var kategorija = kategorije.First(
                    k => k.Id == program.KategorijaProgramaId);

                var predavac = predavaci.First(
                    p => p.Id == program.PredavacId);

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
                    KategorijaProgramaId =
                        program.KategorijaProgramaId,
                    NazivKategorije = kategorija.Naziv,
                    PredavacId = program.PredavacId,
                    ImeIPrezimePredavaca =
                        $"{predavac.Ime} {predavac.Prezime}"
                };
            });
        }
    }
}