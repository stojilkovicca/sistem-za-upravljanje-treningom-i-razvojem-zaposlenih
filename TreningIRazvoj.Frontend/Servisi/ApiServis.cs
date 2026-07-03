using System.Net.Http.Headers;

namespace TreningIRazvoj.Frontend.Servisi
{
    public class ApiServis : IApiServis
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiServis(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient KreirajKlijentaSaTokenom()
        {
            var klijent = _httpClientFactory.CreateClient(
                "TreningIRazvojAPI");

            var token = _httpContextAccessor
                .HttpContext?
                .Session
                .GetString("JwtToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                klijent.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            return klijent;
        }

        public HttpClient KreirajKlijentaBezTokena()
        {
            return _httpClientFactory.CreateClient(
                "TreningIRazvojAPI");
        }
    }
}