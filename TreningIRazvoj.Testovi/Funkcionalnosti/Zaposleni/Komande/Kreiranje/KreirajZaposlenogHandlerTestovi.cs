using Moq;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Kreiranje;
using TreningIRazvoj.Domen.Interfejsi;
using ZaposleniEntitet = TreningIRazvoj.Domen.Modeli.Zaposleni;

namespace TreningIRazvoj.Testovi.Funkcionalnosti.Zaposleni.Komande.Kreiranje
{
    public class KreirajZaposlenogHandlerTestovi
    {
        private readonly Mock<IJedinicaRada> _jedinicaRadaMock;

        private readonly Mock<IRepozitorijum<ZaposleniEntitet>>
            _repozitorijumMock;

        private readonly KreirajZaposlenogHandler _handler;

        public KreirajZaposlenogHandlerTestovi()
        {
            _jedinicaRadaMock = new Mock<IJedinicaRada>();

            _repozitorijumMock =
                new Mock<IRepozitorijum<ZaposleniEntitet>>();

            _jedinicaRadaMock
                .Setup(j => j.Zaposleni)
                .Returns(_repozitorijumMock.Object);

            _handler = new KreirajZaposlenogHandler(
                _jedinicaRadaMock.Object);
        }

        [Fact]
        public async Task Handle_IspravniPodaci_KreiraZaposlenog()
        {
            _repozitorijumMock
                .Setup(r => r.VratiSve())
                .ReturnsAsync(new List<ZaposleniEntitet>());

            ZaposleniEntitet? dodatiZaposleni = null;

            _repozitorijumMock
                .Setup(r => r.Dodaj(It.IsAny<ZaposleniEntitet>()))
                .Callback<ZaposleniEntitet>(z =>
                {
                    z.Id = 10;
                    dodatiZaposleni = z;
                })
                .Returns(Task.CompletedTask);

            _jedinicaRadaMock
                .Setup(j => j.SacuvajPromene())
                .ReturnsAsync(1);

            var komanda = new KreirajZaposlenogKomanda
            {
                Podaci = KreirajIspravnePodatke()
            };

            var rezultat = await _handler.Handle(
                komanda,
                CancellationToken.None);

            Assert.NotNull(rezultat);
            Assert.Equal(10, rezultat.Id);
            Assert.Equal("Marko", rezultat.Ime);
            Assert.Equal("Marković", rezultat.Prezime);
            Assert.Equal(
                "marko.markovic@firma.rs",
                rezultat.Imejl);

            Assert.NotNull(dodatiZaposleni);
            Assert.Equal(
                komanda.Podaci.RadnoMesto,
                dodatiZaposleni.RadnoMesto);

            _repozitorijumMock.Verify(
                r => r.Dodaj(It.IsAny<ZaposleniEntitet>()),
                Times.Once);

            _jedinicaRadaMock.Verify(
                j => j.SacuvajPromene(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ImejlVecPostoji_BacaIzuzetak()
        {
            var postojeciZaposleni = new ZaposleniEntitet
            {
                Id = 1,
                Ime = "Petar",
                Prezime = "Petrović",
                Imejl = "marko.markovic@firma.rs",
                RadnoMesto = "Programer",
                Odeljenje = "IT",
                DatumZaposlenja = DateTime.Today.AddYears(-2),
                Aktivan = true
            };

            _repozitorijumMock
                .Setup(r => r.VratiSve())
                .ReturnsAsync(new List<ZaposleniEntitet>
                {
                    postojeciZaposleni
                });

            var komanda = new KreirajZaposlenogKomanda
            {
                Podaci = KreirajIspravnePodatke()
            };

            var izuzetak =
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _handler.Handle(
                        komanda,
                        CancellationToken.None));

            Assert.Equal(
                "Zaposleni sa unetom imejl adresom već postoji.",
                izuzetak.Message);

            _repozitorijumMock.Verify(
                r => r.Dodaj(It.IsAny<ZaposleniEntitet>()),
                Times.Never);

            _jedinicaRadaMock.Verify(
                j => j.SacuvajPromene(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ImejlPostojiSaDrugacijimVelikimSlovima_BacaIzuzetak()
        {
            var postojeciZaposleni = new ZaposleniEntitet
            {
                Id = 1,
                Ime = "Petar",
                Prezime = "Petrović",
                Imejl = "MARKO.MARKOVIC@FIRMA.RS",
                RadnoMesto = "Programer",
                Odeljenje = "IT",
                DatumZaposlenja = DateTime.Today.AddYears(-2),
                Aktivan = true
            };

            _repozitorijumMock
                .Setup(r => r.VratiSve())
                .ReturnsAsync(new List<ZaposleniEntitet>
                {
                    postojeciZaposleni
                });

            var komanda = new KreirajZaposlenogKomanda
            {
                Podaci = KreirajIspravnePodatke()
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(
                    komanda,
                    CancellationToken.None));

            _repozitorijumMock.Verify(
                r => r.Dodaj(It.IsAny<ZaposleniEntitet>()),
                Times.Never);

            _jedinicaRadaMock.Verify(
                j => j.SacuvajPromene(),
                Times.Never);
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