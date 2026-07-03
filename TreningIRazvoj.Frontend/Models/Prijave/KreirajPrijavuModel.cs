using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TreningIRazvoj.Frontend.Models.Prijave
{
    public class KreirajPrijavuModel
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Izaberite zaposlenog.")]
        [Display(Name = "Zaposleni")]
        public int ZaposleniId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Izaberite razvojni program.")]
        [Display(Name = "Razvojni program")]
        public int RazvojniProgramId { get; set; }

        public IEnumerable<SelectListItem> Zaposleni { get; set; }
            = [];

        public IEnumerable<SelectListItem> RazvojniProgrami { get; set; }
            = [];
    }
}