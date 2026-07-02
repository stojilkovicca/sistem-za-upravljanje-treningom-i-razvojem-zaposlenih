using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreningIRazvoj.Domen.Modeli;

namespace TreningIRazvoj.Domen.Interfejsi
{
    public interface IJedinicaRada
    {
        IRepozitorijum<Zaposleni> Zaposleni { get; }

        IRepozitorijum<Predavac> Predavaci { get; }

        IRepozitorijum<KategorijaPrograma> KategorijePrograma { get; }

        IRepozitorijum<RazvojniProgram> RazvojniProgrami { get; }

        IRepozitorijumPrijava Prijave { get; }

        Task<int> SacuvajPromene();
    }
}
