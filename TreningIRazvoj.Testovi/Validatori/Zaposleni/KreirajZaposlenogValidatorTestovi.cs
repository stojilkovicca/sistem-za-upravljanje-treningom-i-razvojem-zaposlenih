using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.API.Validatori.Zaposleni;

namespace TreningIRazvoj.Testovi.Validatori.Zaposleni
{
    public class KreirajZaposlenogValidatorTestovi
    {
        private readonly KreirajZaposlenogValidator _validator;

        public KreirajZaposlenogValidatorTestovi()
        {
            _validator = new KreirajZaposlenogValidator();
        }

        [Fact]
        public async Task Validacija_IspravniPodaci_VracaUspeh()
        {
            var podaci = new KreirajZaposlenogDTO
            {
                Ime = "Marko",
                Prezime = "Marković",
                Imejl = "marko.markovic@firma.rs",
                RadnoMesto = "Softverski inženjer",
                Odeljenje = "Informacione tehnologije",
                DatumZaposlenja = DateTime.Today.AddYears(-1),
                Aktivan = true
            };

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.True(rezultat.IsValid);
            Assert.Empty(rezultat.Errors);
        }

        [Fact]
        public async Task Validacija_PraznoIme_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.Ime = string.Empty;

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName == nameof(podaci.Ime));
        }

        [Fact]
        public async Task Validacija_NeispravanImejl_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.Imejl = "neispravan-imejl";

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName == nameof(podaci.Imejl));
        }

        [Fact]
        public async Task Validacija_PraznoRadnoMesto_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.RadnoMesto = string.Empty;

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName == nameof(podaci.RadnoMesto));
        }

        [Fact]
        public async Task Validacija_PraznoOdeljenje_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.Odeljenje = string.Empty;

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName == nameof(podaci.Odeljenje));
        }

        [Fact]
        public async Task Validacija_DatumUBuducnosti_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.DatumZaposlenja = DateTime.Today.AddDays(1);

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName ==
                          nameof(podaci.DatumZaposlenja));
        }

        [Fact]
        public async Task Validacija_PrazanDatumZaposlenja_VracaGresku()
        {
            var podaci = KreirajIspravnePodatke();
            podaci.DatumZaposlenja = default;

            var rezultat = await _validator.ValidateAsync(podaci);

            Assert.False(rezultat.IsValid);
            Assert.Contains(
                rezultat.Errors,
                greska => greska.PropertyName ==
                          nameof(podaci.DatumZaposlenja));
        }

        private static KreirajZaposlenogDTO KreirajIspravnePodatke()
        {
            return new KreirajZaposlenogDTO
            {
                Ime = "Marko",
                Prezime = "Marković",
                Imejl = "marko.markovic@firma.rs",
                RadnoMesto = "Softverski inženjer",
                Odeljenje = "Informacione tehnologije",
                DatumZaposlenja = DateTime.Today.AddYears(-1),
                Aktivan = true
            };
        }
    }
}