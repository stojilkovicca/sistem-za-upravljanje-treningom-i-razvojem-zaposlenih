using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.Domen.Modeli
{
    public class Prijava
    {
        public int ZaposleniId { get; set; }

        public Zaposleni Zaposleni { get; set; } = null!;

        public int RazvojniProgramId { get; set; }

        public RazvojniProgram RazvojniProgram { get; set; } = null!;

        public DateTime DatumPrijave { get; set; }

        public StatusPrijave Status { get; set; }

        public decimal? ProcenatPrisustva { get; set; }

        public int? BrojPoena { get; set; }

        public DateTime? DatumZavrsetka { get; set; }

        public int? OcenaPrograma { get; set; }
    }
}
