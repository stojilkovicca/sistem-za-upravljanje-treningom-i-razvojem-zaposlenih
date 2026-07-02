using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreningIRazvoj.Domen.Modeli
{
    public class Predavac
    {
        public int Id { get; set; }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Imejl { get; set; } = string.Empty;

        public string OblastStrucnosti { get; set; } = string.Empty;

        public bool Interni { get; set; }

        public ICollection<RazvojniProgram> RazvojniProgrami { get; set; }
            = new List<RazvojniProgram>();
    }
}
