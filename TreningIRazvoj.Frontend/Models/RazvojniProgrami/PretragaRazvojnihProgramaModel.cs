using Microsoft.AspNetCore.Mvc.Rendering;
using TreningIRazvoj.Frontend.Models.Opste;

namespace TreningIRazvoj.Frontend.Models.RazvojniProgrami
{
    public class PretragaRazvojnihProgramaModel
    {
        public string? Pretraga { get; set; }

        public VrstaProgramaModel? Vrsta { get; set; }

        public int? KategorijaProgramaId { get; set; }

        public int? PredavacId { get; set; }

        public bool? Objavljen { get; set; }

        public string SortirajPo { get; set; } = "datumPocetka";

        public string SmerSortiranja { get; set; } = "rastuce";

        public int BrojStranice { get; set; } = 1;

        public int VelicinaStranice { get; set; } = 10;

        public StranicniRezultatModel<RazvojniProgramModel> Rezultat
        { get; set; } = new();

        public IEnumerable<SelectListItem> Kategorije { get; set; } = [];

        public IEnumerable<SelectListItem> Predavaci { get; set; } = [];
    }
}