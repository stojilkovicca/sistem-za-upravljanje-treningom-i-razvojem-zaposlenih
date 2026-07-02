using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.API.DTO.RazvojniProgrami
{
    public class RazvojniProgramDTO
    {
        public int Id { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public VrstaPrograma Vrsta { get; set; }

        public DateTime DatumPocetka { get; set; }

        public DateTime DatumZavrsetka { get; set; }

        public int Kapacitet { get; set; }

        public int TrajanjeUSatima { get; set; }

        public int? MinimalanBrojPoena { get; set; }

        public bool Objavljen { get; set; }

        public int KategorijaProgramaId { get; set; }

        public string NazivKategorije { get; set; } = string.Empty;

        public int PredavacId { get; set; }

        public string ImeIPrezimePredavaca { get; set; } = string.Empty;
    }
}
