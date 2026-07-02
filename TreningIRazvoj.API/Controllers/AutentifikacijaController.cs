using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.Autentifikacija;
using TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Prijavljivanje;
using TreningIRazvoj.API.Funkcionalnosti.Autentifikacija.Komande.Registracija;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/autentifikacija")]
    public class AutentifikacijaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutentifikacijaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("registracija")]
        [Authorize(Roles = Uloge.Administrator)]
        public async Task<IActionResult> Registracija(
            [FromBody] RegistracijaDTO podaci,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new RegistrujKorisnikaKomanda
                {
                    Podaci = podaci
                },
                cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    poruka = "Korisnik je uspešno registrovan."
                });
        }

        [HttpPost("prijava")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDTO>> Prijava(
            [FromBody] PrijavaKorisnikaDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new PrijaviKorisnikaKomanda
                {
                    Podaci = podaci
                },
                cancellationToken);

            return Ok(rezultat);
        }
    }
}