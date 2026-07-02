using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Izmena
{
    public class IzmeniRazvojniProgramHandler
        : IRequestHandler<
            IzmeniRazvojniProgramKomanda,
            RazvojniProgramDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public IzmeniRazvojniProgramHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<RazvojniProgramDTO> Handle(
            IzmeniRazvojniProgramKomanda zahtev,
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
                    .VratiPoId(zahtev.Podaci.KategorijaProgramaId);

            if (kategorija is null)
            {
                throw new KeyNotFoundException(
                    $"Kategorija programa sa identifikatorom {zahtev.Podaci.KategorijaProgramaId} nije pronađena.");
            }

            var predavac =
                await _jedinicaRada.Predavaci
                    .VratiPoId(zahtev.Podaci.PredavacId);

            if (predavac is null)
            {
                throw new KeyNotFoundException(
                    $"Predavač sa identifikatorom {zahtev.Podaci.PredavacId} nije pronađen.");
            }

            program.Naziv = zahtev.Podaci.Naziv;
            program.Opis = zahtev.Podaci.Opis;
            program.Vrsta = zahtev.Podaci.Vrsta;
            program.DatumPocetka = zahtev.Podaci.DatumPocetka;
            program.DatumZavrsetka = zahtev.Podaci.DatumZavrsetka;
            program.Kapacitet = zahtev.Podaci.Kapacitet;
            program.TrajanjeUSatima = zahtev.Podaci.TrajanjeUSatima;
            program.MinimalanBrojPoena =
                zahtev.Podaci.MinimalanBrojPoena;
            program.Objavljen = zahtev.Podaci.Objavljen;
            program.KategorijaProgramaId =
                zahtev.Podaci.KategorijaProgramaId;
            program.PredavacId = zahtev.Podaci.PredavacId;

            _jedinicaRada.RazvojniProgrami.Izmeni(program);
            await _jedinicaRada.SacuvajPromene();

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
        }
    }
}
