namespace TreningIRazvoj.API.DTO.Opste
{
    public class StranicniRezultat<T>
    {
        public IEnumerable<T> Stavke { get; set; } = [];

        public int UkupanBrojStavki { get; set; }

        public int BrojStranice { get; set; }

        public int VelicinaStranice { get; set; }

        public int UkupanBrojStranica { get; set; }
    }
}