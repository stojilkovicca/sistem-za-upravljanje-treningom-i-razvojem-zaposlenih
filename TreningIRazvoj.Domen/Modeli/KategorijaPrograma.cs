using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreningIRazvoj.Domen.Modeli
{
    public class KategorijaPrograma
    {
        public int Id { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public ICollection<RazvojniProgram> RazvojniProgrami { get; set; }
            = new List<RazvojniProgram>();
    }
}
