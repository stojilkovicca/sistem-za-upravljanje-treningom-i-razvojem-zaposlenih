using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Predavaci
{
    public class PredavacModel
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

        [Required(ErrorMessage = "Oblast stručnosti je obavezna.")]
        [Display(Name = "Oblast stručnosti")]
        [StringLength(150)]
        public string OblastStrucnosti { get; set; } = string.Empty;

        [Display(Name = "Interni predavač")]
        public bool Interni { get; set; }
    }
}