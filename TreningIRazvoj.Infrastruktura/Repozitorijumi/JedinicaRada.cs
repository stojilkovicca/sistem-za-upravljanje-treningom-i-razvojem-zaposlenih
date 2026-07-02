using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreningIRazvoj.Domen.Interfejsi;
using TreningIRazvoj.Domen.Modeli;
using TreningIRazvoj.Infrastruktura.Podaci;

namespace TreningIRazvoj.Infrastruktura.Repozitorijumi
{
    public class JedinicaRada : IJedinicaRada
    {
        private readonly TreningIRazvojKontekst _kontekst;

        public IRepozitorijum<Zaposleni> Zaposleni { get; }

        public IRepozitorijum<Predavac> Predavaci { get; }

        public IRepozitorijum<KategorijaPrograma> KategorijePrograma { get; }

        public IRepozitorijum<RazvojniProgram> RazvojniProgrami { get; }

        public IRepozitorijumPrijava Prijave { get; }

        public JedinicaRada(TreningIRazvojKontekst kontekst)
        {
            _kontekst = kontekst;

            Zaposleni = new Repozitorijum<Zaposleni>(kontekst);
            Predavaci = new Repozitorijum<Predavac>(kontekst);
            KategorijePrograma =
                new Repozitorijum<KategorijaPrograma>(kontekst);
            RazvojniProgrami =
                new Repozitorijum<RazvojniProgram>(kontekst);
            Prijave = new RepozitorijumPrijava(kontekst);
        }

        public async Task<int> SacuvajPromene()
        {
            return await _kontekst.SaveChangesAsync();
        }
    }
}
