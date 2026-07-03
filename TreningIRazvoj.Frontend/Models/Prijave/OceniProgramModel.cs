using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Prijave
{
    public class OceniProgramModel
    {
        public int ZaposleniId { get; set; }

        public int RazvojniProgramId { get; set; }

        public string ImeIPrezimeZaposlenog { get; set; }
            = string.Empty;

        public string NazivRazvojnogPrograma { get; set; }
            = string.Empty;

        [Range(1, 5,
            ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena programa")]
        public int OcenaPrograma { get; set; } = 5;
    }
}