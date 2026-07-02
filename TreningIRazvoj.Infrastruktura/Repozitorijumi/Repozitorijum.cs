using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using TreningIRazvoj.Domen.Interfejsi;
using TreningIRazvoj.Infrastruktura.Podaci;

namespace TreningIRazvoj.Infrastruktura.Repozitorijumi
{
    public class Repozitorijum<T> : IRepozitorijum<T> where T : class
    {
        protected readonly TreningIRazvojKontekst _kontekst;
        protected readonly DbSet<T> _skup;

        public Repozitorijum(TreningIRazvojKontekst kontekst)
        {
            _kontekst = kontekst;
            _skup = kontekst.Set<T>();
        }

        public async Task<IEnumerable<T>> VratiSve()
        {
            return await _skup.ToListAsync();
        }

        public async Task<T?> VratiPoId(int id)
        {
            return await _skup.FindAsync(id);
        }

        public async Task Dodaj(T entitet)
        {
            await _skup.AddAsync(entitet);
        }

        public void Izmeni(T entitet)
        {
            _skup.Update(entitet);
        }

        public void Obrisi(T entitet)
        {
            _skup.Remove(entitet);
        }
    }
}
