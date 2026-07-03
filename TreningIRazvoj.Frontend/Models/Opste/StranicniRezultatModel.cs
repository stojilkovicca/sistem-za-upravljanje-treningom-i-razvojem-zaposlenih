namespace TreningIRazvoj.Frontend.Models.Opste
{
    public class StranicniRezultatModel<T>
    {
        public IEnumerable<T> Stavke { get; set; } = [];

        public int UkupanBrojStavki { get; set; }

        public int BrojStranice { get; set; }

        public int VelicinaStranice { get; set; }

        public int UkupanBrojStranica { get; set; }
    }
}