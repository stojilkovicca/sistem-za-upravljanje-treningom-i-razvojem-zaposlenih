using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.API.DTO.Prijave
{
    public class ObradiPrijavuDTO
    {
        public StatusPrijave Status { get; set; }

        public decimal? ProcenatPrisustva { get; set; }

        public int? BrojPoena { get; set; }

        public DateTime? DatumZavrsetka { get; set; }
    }
}
