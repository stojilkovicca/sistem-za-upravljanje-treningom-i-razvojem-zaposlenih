using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Zaposleni
{
    public class ZaposleniModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(50)]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(50)]
        public string Prezime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Imejl je obavezan.")]
        [EmailAddress(ErrorMessage = "Imejl nije ispravan.")]
        [StringLength(150)]
        public string Imejl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Radno mesto je obavezno.")]
        [Display(Name = "Radno mesto")]
        [StringLength(100)]
        public string RadnoMesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Odeljenje je obavezno.")]
        [StringLength(100)]
        public string Odeljenje { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum zaposlenja je obavezan.")]
        [Display(Name = "Datum zaposlenja")]
        [DataType(DataType.Date)]
        public DateTime DatumZaposlenja { get; set; } = DateTime.Today;

        public bool Aktivan { get; set; } = true;
    }
}