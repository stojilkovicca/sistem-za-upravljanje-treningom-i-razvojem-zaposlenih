using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.RazvojniProgrami
{
    public class RazvojniProgramModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(150)]
        public string Naziv { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis je obavezan.")]
        [StringLength(1000)]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Vrsta programa")]
        public VrstaProgramaModel Vrsta { get; set; }

        [Required]
        [Display(Name = "Datum početka")]
        [DataType(DataType.Date)]
        public DateTime DatumPocetka { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Datum završetka")]
        [DataType(DataType.Date)]
        public DateTime DatumZavrsetka { get; set; } =
            DateTime.Today.AddDays(1);

        [Range(1, int.MaxValue, ErrorMessage = "Kapacitet mora biti veći od nule.")]
        public int Kapacitet { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Trajanje mora biti veće od nule.")]
        [Display(Name = "Trajanje u satima")]
        public int TrajanjeUSatima { get; set; }

        [Display(Name = "Minimalan broj poena")]
        public int? MinimalanBrojPoena { get; set; }

        public bool Objavljen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Izaberite kategoriju.")]
        [Display(Name = "Kategorija")]
        public int KategorijaProgramaId { get; set; }

        public string NazivKategorije { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Izaberite predavača.")]
        [Display(Name = "Predavač")]
        public int PredavacId { get; set; }

        public string ImeIPrezimePredavaca { get; set; } = string.Empty;
    }
}