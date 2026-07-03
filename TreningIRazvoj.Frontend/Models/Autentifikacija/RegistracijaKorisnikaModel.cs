using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Autentifikacija
{
    public class RegistracijaKorisnikaModel
    {
        [Required(ErrorMessage = "Ime je obavezno.")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezime je obavezno.")]
        public string Prezime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Imejl je obavezan.")]
        [EmailAddress(ErrorMessage = "Imejl nije ispravan.")]
        public string Imejl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [MinLength(8, ErrorMessage = "Lozinka mora imati najmanje 8 karaktera.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; } = string.Empty;

        [Required(ErrorMessage = "Uloga je obavezna.")]
        public string Uloga { get; set; } = "Zaposleni";
    }
}