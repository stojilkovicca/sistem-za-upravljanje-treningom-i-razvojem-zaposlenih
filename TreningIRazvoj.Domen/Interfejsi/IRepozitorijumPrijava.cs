using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreningIRazvoj.Domen.Modeli;

namespace TreningIRazvoj.Domen.Interfejsi
{
    public interface IRepozitorijumPrijava
    {
        Task<IEnumerable<Prijava>> VratiSve();

        Task<Prijava?> VratiPoId(
            int zaposleniId,
            int razvojniProgramId);

        Task Dodaj(Prijava prijava);

        void Izmeni(Prijava prijava);

        void Obrisi(Prijava prijava);
    }
}
