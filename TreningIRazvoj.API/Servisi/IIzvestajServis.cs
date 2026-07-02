using TreningIRazvoj.API.DTO.Prijave;

namespace TreningIRazvoj.API.Servisi
{
    public interface IIzvestajServis
    {
        byte[] GenerisiIzvestajOPrijavama(
            IEnumerable<PrijavaDTO> prijave);
    }
}