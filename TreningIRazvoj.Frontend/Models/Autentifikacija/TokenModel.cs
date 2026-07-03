namespace TreningIRazvoj.Frontend.Models.Autentifikacija
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Istice { get; set; }

        public string Imejl { get; set; } = string.Empty;

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public IEnumerable<string> Uloge { get; set; }
            = Enumerable.Empty<string>();
    }
}