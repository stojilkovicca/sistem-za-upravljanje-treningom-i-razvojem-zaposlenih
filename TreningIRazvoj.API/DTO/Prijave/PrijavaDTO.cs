using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.API.DTO.Prijave
{
    public class PrijavaDTO
    {
        public int ZaposleniId { get; set; }

        public string ImeIPrezimeZaposlenog { get; set; } = string.Empty;

        public int RazvojniProgramId { get; set; }

        public string NazivRazvojnogPrograma { get; set; } = string.Empty;

        public DateTime DatumPrijave { get; set; }

        public StatusPrijave Status { get; set; }

        public decimal? ProcenatPrisustva { get; set; }

        public int? BrojPoena { get; set; }

        public DateTime? DatumZavrsetka { get; set; }

        public int? OcenaPrograma { get; set; }
    }
}
