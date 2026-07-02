using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreningIRazvoj.Domen.Interfejsi
{
    public interface IRepozitorijum<T> where T : class
    {
        Task<IEnumerable<T>> VratiSve();

        Task<T?> VratiPoId(int id);

        Task Dodaj(T entitet);

        void Izmeni(T entitet);

        void Obrisi(T entitet);
    }
}
