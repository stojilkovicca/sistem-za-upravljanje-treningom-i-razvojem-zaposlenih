namespace TreningIRazvoj.API.Podesavanja
{
    public class JwtPodesavanja
    {
        public string Kljuc { get; set; } = string.Empty;

        public string Izdavalac { get; set; } = string.Empty;

        public string Primalac { get; set; } = string.Empty;

        public int TrajanjeUMinutima { get; set; }
    }
}