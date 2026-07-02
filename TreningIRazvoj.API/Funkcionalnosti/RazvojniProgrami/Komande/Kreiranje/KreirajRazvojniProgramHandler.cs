using MediatR;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Interfejsi;
using RazvojniProgramEntitet =
    TreningIRazvoj.Domen.Modeli.RazvojniProgram;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Kreiranje
{
    public class KreirajRazvojniProgramHandler
        : IRequestHandler<
            KreirajRazvojniProgramKomanda,
            RazvojniProgramDTO>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public KreirajRazvojniProgramHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<RazvojniProgramDTO> Handle(
            KreirajRazvojniProgramKomanda zahtev,
            CancellationToken cancellationToken)
        {
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

            var program = new RazvojniProgramEntitet
            {
                Naziv = zahtev.Podaci.Naziv,
                Opis = zahtev.Podaci.Opis,
                Vrsta = zahtev.Podaci.Vrsta,
                DatumPocetka = zahtev.Podaci.DatumPocetka,
                DatumZavrsetka = zahtev.Podaci.DatumZavrsetka,
                Kapacitet = zahtev.Podaci.Kapacitet,
                TrajanjeUSatima = zahtev.Podaci.TrajanjeUSatima,
                MinimalanBrojPoena = zahtev.Podaci.MinimalanBrojPoena,
                Objavljen = zahtev.Podaci.Objavljen,
                KategorijaProgramaId =
                    zahtev.Podaci.KategorijaProgramaId,
                PredavacId = zahtev.Podaci.PredavacId
            };

            await _jedinicaRada.RazvojniProgrami.Dodaj(program);
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
                KategorijaProgramaId = program.KategorijaProgramaId,
                NazivKategorije = kategorija.Naziv,
                PredavacId = program.PredavacId,
                ImeIPrezimePredavaca =
                    $"{predavac.Ime} {predavac.Prezime}"
            };
        }
    }
}