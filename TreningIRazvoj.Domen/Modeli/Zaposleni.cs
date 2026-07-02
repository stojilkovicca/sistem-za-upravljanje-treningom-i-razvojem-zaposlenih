using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreningIRazvoj.Domen.Modeli
{
    public class Zaposleni
    {
        public int Id { get; set; }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Imejl { get; set; } = string.Empty;

        public string RadnoMesto { get; set; } = string.Empty;

        public string Odeljenje { get; set; } = string.Empty;

        public DateTime DatumZaposlenja { get; set; }

        public bool Aktivan { get; set; }

        public ICollection<Prijava> Prijave { get; set; }
            = new List<Prijava>();
    }
}
