namespace TreningIRazvoj.API.DTO.Predavaci
{
    public class PredavacDTO
    {
        public int Id { get; set; }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Imejl { get; set; } = string.Empty;

        public string OblastStrucnosti { get; set; } = string.Empty;

        public bool Interni { get; set; }
    }
}
