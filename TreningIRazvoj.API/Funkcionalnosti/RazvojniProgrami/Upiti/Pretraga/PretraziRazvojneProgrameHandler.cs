using MediatR;
using TreningIRazvoj.API.DTO.Opste;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Interfejsi;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.Pretraga
{
    public class PretraziRazvojneProgrameHandler
        : IRequestHandler<
            PretraziRazvojneProgrameUpit,
            StranicniRezultat<RazvojniProgramDTO>>
    {
        private readonly IJedinicaRada _jedinicaRada;

        public PretraziRazvojneProgrameHandler(
            IJedinicaRada jedinicaRada)
        {
            _jedinicaRada = jedinicaRada;
        }

        public async Task<StranicniRezultat<RazvojniProgramDTO>> Handle(
            PretraziRazvojneProgrameUpit zahtev,
            CancellationToken cancellationToken)
        {
            var programi =
                (await _jedinicaRada.RazvojniProgrami.VratiSve())
                .AsEnumerable();

            var kategorije =
                await _jedinicaRada.KategorijePrograma.VratiSve();

            var predavaci =
                await _jedinicaRada.Predavaci.VratiSve();

            if (!string.IsNullOrWhiteSpace(zahtev.Pretraga))
            {
                string tekst = zahtev.Pretraga.Trim();

                programi = programi.Where(p =>
                    p.Naziv.Contains(
                        tekst,
                        StringComparison.OrdinalIgnoreCase) ||
                    p.Opis.Contains(
                        tekst,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (zahtev.Vrsta.HasValue)
            {
                programi = programi.Where(
                    p => p.Vrsta == zahtev.Vrsta.Value);
            }

            if (zahtev.KategorijaProgramaId.HasValue)
            {
                programi = programi.Where(p =>
                    p.KategorijaProgramaId ==
                    zahtev.KategorijaProgramaId.Value);
            }

            if (zahtev.PredavacId.HasValue)
            {
                programi = programi.Where(p =>
                    p.PredavacId == zahtev.PredavacId.Value);
            }

            if (zahtev.Objavljen.HasValue)
            {
                programi = programi.Where(
                    p => p.Objavljen == zahtev.Objavljen.Value);
            }

            bool opadajuce =
                zahtev.SmerSortiranja == "opadajuce";

            programi = zahtev.SortirajPo switch
            {
                "naziv" => opadajuce
                    ? programi.OrderByDescending(p => p.Naziv)
                    : programi.OrderBy(p => p.Naziv),

                "datumZavrsetka" => opadajuce
                    ? programi.OrderByDescending(p => p.DatumZavrsetka)
                    : programi.OrderBy(p => p.DatumZavrsetka),

                "kapacitet" => opadajuce
                    ? programi.OrderByDescending(p => p.Kapacitet)
                    : programi.OrderBy(p => p.Kapacitet),

                "trajanjeUSatima" => opadajuce
                    ? programi.OrderByDescending(p => p.TrajanjeUSatima)
                    : programi.OrderBy(p => p.TrajanjeUSatima),

                _ => opadajuce
                    ? programi.OrderByDescending(p => p.DatumPocetka)
                    : programi.OrderBy(p => p.DatumPocetka)
            };

            int ukupanBrojStavki = programi.Count();

            var programiSaStranice = programi
                .Skip(
                    (zahtev.BrojStranice - 1) *
                    zahtev.VelicinaStranice)
                .Take(zahtev.VelicinaStranice)
                .ToList();

            var stavke = programiSaStranice.Select(program =>
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
                    MinimalanBrojPoena =
                        program.MinimalanBrojPoena,
                    Objavljen = program.Objavljen,
                    KategorijaProgramaId =
                        program.KategorijaProgramaId,
                    NazivKategorije = kategorija.Naziv,
                    PredavacId = program.PredavacId,
                    ImeIPrezimePredavaca =
                        $"{predavac.Ime} {predavac.Prezime}"
                };
            });

            return new StranicniRezultat<RazvojniProgramDTO>
            {
                Stavke = stavke,
                UkupanBrojStavki = ukupanBrojStavki,
                BrojStranice = zahtev.BrojStranice,
                VelicinaStranice = zahtev.VelicinaStranice,
                UkupanBrojStranica = (int)Math.Ceiling(
                    ukupanBrojStavki /
                    (double)zahtev.VelicinaStranice)
            };
        }
    }
}