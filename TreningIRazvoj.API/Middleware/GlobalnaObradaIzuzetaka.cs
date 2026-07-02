using FluentValidation;
using System.Net;
using System.Text.Json;

namespace TreningIRazvoj.API.Middleware
{
    public class GlobalnaObradaIzuzetaka
    {
        private readonly RequestDelegate _sledeci;
        private readonly ILogger<GlobalnaObradaIzuzetaka> _logger;

        public GlobalnaObradaIzuzetaka(
            RequestDelegate sledeci,
            ILogger<GlobalnaObradaIzuzetaka> logger)
        {
            _sledeci = sledeci;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext kontekst)
        {
            try
            {
                await _sledeci(kontekst);
            }
            catch (Exception izuzetak)
            {
                await ObradiIzuzetak(kontekst, izuzetak);
            }
        }

        private async Task ObradiIzuzetak(
            HttpContext kontekst,
            Exception izuzetak)
        {
            _logger.LogError(
                izuzetak,
                "Došlo je do greške prilikom obrade zahteva.");

            kontekst.Response.ContentType = "application/json";

            object odgovor;

            switch (izuzetak)
            {
                case ValidationException validacioniIzuzetak:
                    kontekst.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;

                    odgovor = new
                    {
                        poruka = "Podaci nisu ispravni.",
                        greske = validacioniIzuzetak.Errors
                            .GroupBy(g => g.PropertyName)
                            .ToDictionary(
                                grupa => grupa.Key,
                                grupa => grupa
                                    .Select(g => g.ErrorMessage)
                                    .ToArray())
                    };
                    break;

                case KeyNotFoundException:
                    kontekst.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;

                    odgovor = new
                    {
                        poruka = izuzetak.Message
                    };
                    break;

                case InvalidOperationException:
                    kontekst.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;

                    odgovor = new
                    {
                        poruka = izuzetak.Message
                    };
                    break;

                case UnauthorizedAccessException:
                    kontekst.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;

                    odgovor = new
                    {
                        poruka = izuzetak.Message
                    };
                    break;

                default:
                    kontekst.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    odgovor = new
                    {
                        poruka = "Došlo je do neočekivane greške na serveru."
                    };
                    break;
            }

            var json = JsonSerializer.Serialize(odgovor);

            await kontekst.Response.WriteAsync(json);
        }
    }
}