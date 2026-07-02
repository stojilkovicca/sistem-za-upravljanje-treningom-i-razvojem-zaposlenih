using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Servisi
{
    public class IzvestajServis : IIzvestajServis
    {
        public byte[] GenerisiIzvestajOPrijavama(
            IEnumerable<PrijavaDTO> prijave)
        {
            var listaPrijava = prijave.ToList();

            var dokument = Document.Create(kontejner =>
            {
                kontejner.Page(stranica =>
                {
                    stranica.Size(PageSizes.A4);
                    stranica.Margin(30);

                    stranica.DefaultTextStyle(
                        stil => stil.FontSize(10));

                    stranica.Header()
                        .Column(kolona =>
                        {
                            kolona.Item()
                                .Text("Izveštaj o prijavama na razvojne programe")
                                .FontSize(18)
                                .Bold();

                            kolona.Item()
                                .Text(
                                    $"Datum generisanja: {DateTime.Now:dd.MM.yyyy. HH:mm}");
                        });

                    stranica.Content()
                        .PaddingVertical(20)
                        .Column(kolona =>
                        {
                            kolona.Spacing(15);

                            kolona.Item()
                                .Text(
                                    $"Ukupan broj prijava: {listaPrijava.Count}")
                                .Bold();

                            if (listaPrijava.Count == 0)
                            {
                                kolona.Item()
                                    .Text("Nema evidentiranih prijava.");

                                return;
                            }

                            kolona.Item()
                                .Table(tabela =>
                                {
                                    tabela.ColumnsDefinition(kolone =>
                                    {
                                        kolone.RelativeColumn(2);
                                        kolone.RelativeColumn(2);
                                        kolone.RelativeColumn(1);
                                        kolone.RelativeColumn(1);
                                        kolone.RelativeColumn(1);
                                        kolone.RelativeColumn(1);
                                    });

                                    tabela.Header(zaglavlje =>
                                    {
                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Zaposleni");

                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Program");

                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Datum prijave");

                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Status");

                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Prisustvo");

                                        zaglavlje.Cell()
                                            .Element(StilZaglavlja)
                                            .Text("Ocena");
                                    });

                                    foreach (var prijava in listaPrijava)
                                    {
                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.ImeIPrezimeZaposlenog);

                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.NazivRazvojnogPrograma);

                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.DatumPrijave
                                                    .ToString("dd.MM.yyyy."));

                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.Status.ToString());

                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.ProcenatPrisustva
                                                    .HasValue
                                                        ? $"{prijava.ProcenatPrisustva:0.##}%"
                                                        : "-");

                                        tabela.Cell()
                                            .Element(StilCelije)
                                            .Text(
                                                prijava.OcenaPrograma
                                                    .HasValue
                                                        ? prijava.OcenaPrograma
                                                            .Value
                                                            .ToString()
                                                        : "-");
                                    }
                                });
                        });

                    stranica.Footer()
                        .AlignCenter()
                        .Text(tekst =>
                        {
                            tekst.Span("Strana ");

                            tekst.CurrentPageNumber();

                            tekst.Span(" od ");

                            tekst.TotalPages();
                        });
                });
            });

            return dokument.GeneratePdf();
        }

        private static IContainer StilZaglavlja(
            IContainer kontejner)
        {
            return kontejner
                .Border(1)
                .Background(Colors.Grey.Lighten2)
                .Padding(5)
                .AlignMiddle();
        }

        private static IContainer StilCelije(
            IContainer kontejner)
        {
            return kontejner
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5)
                .AlignMiddle();
        }
    }
}