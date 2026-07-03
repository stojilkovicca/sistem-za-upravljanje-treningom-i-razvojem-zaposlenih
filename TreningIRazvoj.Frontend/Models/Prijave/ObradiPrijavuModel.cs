using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Prijave
{
    public class ObradiPrijavuModel
    {
        public int ZaposleniId { get; set; }

        public int RazvojniProgramId { get; set; }

        public string ImeIPrezimeZaposlenog { get; set; }
            = string.Empty;

        public string NazivRazvojnogPrograma { get; set; }
            = string.Empty;

        [Display(Name = "Status prijave")]
        public StatusPrijaveModel Status { get; set; }

        [Range(0, 100,
            ErrorMessage = "Prisustvo mora biti između 0 i 100.")]
        [Display(Name = "Procenat prisustva")]
        public decimal? ProcenatPrisustva { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Broj poena ne može biti negativan.")]
        [Display(Name = "Broj poena")]
        public int? BrojPoena { get; set; }

        [Display(Name = "Datum završetka")]
        [DataType(DataType.Date)]
        public DateTime? DatumZavrsetka { get; set; }
    }
}