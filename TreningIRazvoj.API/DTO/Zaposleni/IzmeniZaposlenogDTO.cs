namespace TreningIRazvoj.API.DTO.Zaposleni
{
    public class IzmeniZaposlenogDTO
    {
        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Imejl { get; set; } = string.Empty;

        public string RadnoMesto { get; set; } = string.Empty;

        public string Odeljenje { get; set; } = string.Empty;

        public DateTime DatumZaposlenja { get; set; }

        public bool Aktivan { get; set; }
    }
}
