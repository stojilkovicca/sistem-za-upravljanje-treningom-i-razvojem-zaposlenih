using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.Domen.Modeli
{
    public class RazvojniProgram
    {
        public int Id { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public VrstaPrograma Vrsta { get; set; }

        public DateTime DatumPocetka { get; set; }

        public DateTime DatumZavrsetka { get; set; }

        public int Kapacitet { get; set; }

        public int TrajanjeUSatima { get; set; }

        public int? MinimalanBrojPoena { get; set; }

        public bool Objavljen { get; set; }

        public int KategorijaProgramaId { get; set; }

        public KategorijaPrograma KategorijaPrograma { get; set; } = null!;

        public int PredavacId { get; set; }

        public Predavac Predavac { get; set; } = null!;

        public ICollection<Prijava> Prijave { get; set; }
            = new List<Prijava>();
    }
}
