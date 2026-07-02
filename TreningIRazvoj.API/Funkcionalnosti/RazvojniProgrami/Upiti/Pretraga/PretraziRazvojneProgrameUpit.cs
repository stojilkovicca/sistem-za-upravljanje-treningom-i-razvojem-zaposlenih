using MediatR;
using TreningIRazvoj.API.DTO.Opste;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.Pretraga
{
    public class PretraziRazvojneProgrameUpit
        : IRequest<StranicniRezultat<RazvojniProgramDTO>>
    {
        public string? Pretraga { get; set; }

        public VrstaPrograma? Vrsta { get; set; }

        public int? KategorijaProgramaId { get; set; }

        public int? PredavacId { get; set; }

        public bool? Objavljen { get; set; }

        public string SortirajPo { get; set; } = "datumPocetka";

        public string SmerSortiranja { get; set; } = "rastuce";

        public int BrojStranice { get; set; } = 1;

        public int VelicinaStranice { get; set; } = 10;
    }
}
