namespace TreningIRazvoj.API.DTO.Autentifikacija
{
    public class TokenDTO
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Istice { get; set; }

        public string Imejl { get; set; } = string.Empty;

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public IEnumerable<string> Uloge { get; set; } = [];
    }
}