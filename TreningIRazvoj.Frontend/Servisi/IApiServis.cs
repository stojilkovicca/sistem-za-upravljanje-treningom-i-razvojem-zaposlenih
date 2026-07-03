using System.Net.Http.Headers;

namespace TreningIRazvoj.Frontend.Servisi
{
    public interface IApiServis
    {
        HttpClient KreirajKlijentaSaTokenom();

        HttpClient KreirajKlijentaBezTokena();
    }
}