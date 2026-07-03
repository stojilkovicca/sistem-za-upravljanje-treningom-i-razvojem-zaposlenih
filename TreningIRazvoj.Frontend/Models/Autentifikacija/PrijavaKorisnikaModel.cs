using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Autentifikacija
{
    public class PrijavaKorisnikaModel
    {
        [Required(ErrorMessage = "Imejl je obavezan.")]
        [EmailAddress(ErrorMessage = "Imejl nije u ispravnom formatu.")]
        [Display(Name = "Imejl")]
        public string Imejl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; } = string.Empty;
    }
}