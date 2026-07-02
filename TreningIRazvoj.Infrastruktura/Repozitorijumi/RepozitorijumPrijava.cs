using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using TreningIRazvoj.Domen.Interfejsi;
using TreningIRazvoj.Domen.Modeli;
using TreningIRazvoj.Infrastruktura.Podaci;

namespace TreningIRazvoj.Infrastruktura.Repozitorijumi
{
    public class RepozitorijumPrijava : IRepozitorijumPrijava
    {
        private readonly TreningIRazvojKontekst _kontekst;

        public RepozitorijumPrijava(TreningIRazvojKontekst kontekst)
        {
            _kontekst = kontekst;
        }

        public async Task<IEnumerable<Prijava>> VratiSve()
        {
            return await _kontekst.Prijave
                .Include(p => p.Zaposleni)
                .Include(p => p.RazvojniProgram)
                .ToListAsync();
        }

        public async Task<Prijava?> VratiPoId(
            int zaposleniId,
            int razvojniProgramId)
        {
            return await _kontekst.Prijave
                .Include(p => p.Zaposleni)
                .Include(p => p.RazvojniProgram)
                .FirstOrDefaultAsync(p =>
                    p.ZaposleniId == zaposleniId &&
                    p.RazvojniProgramId == razvojniProgramId);
        }

        public async Task Dodaj(Prijava prijava)
        {
            await _kontekst.Prijave.AddAsync(prijava);
        }

        public void Izmeni(Prijava prijava)
        {
            _kontekst.Prijave.Update(prijava);
        }

        public void Obrisi(Prijava prijava)
        {
            _kontekst.Prijave.Remove(prijava);
        }
    }
}
