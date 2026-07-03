using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Kategorije
{
    public class KategorijaProgramaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis je obavezan.")]
        [StringLength(500)]
        public string Opis { get; set; } = string.Empty;
    }
}