using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.Zaposleni;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Brisanje;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Izmena;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Komande.Kreiranje;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiPoId;
using TreningIRazvoj.API.Funkcionalnosti.Zaposleni.Upiti.VratiSve;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/zaposleni")]
    [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
    public class ZaposleniController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ZaposleniController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> VratiSve(
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiSveZaposleneUpit(),
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ZaposleniDTO>> VratiPoId(
            int id,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiZaposlenogPoIdUpit
                {
                    Id = id
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPost]
        public async Task<ActionResult<ZaposleniDTO>> Kreiraj(
            [FromBody] KreirajZaposlenogDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new KreirajZaposlenogKomanda
                {
                    Podaci = podaci
                },
                cancellationToken);

            return CreatedAtAction(
                nameof(VratiPoId),
                new { id = rezultat.Id },
                rezultat);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ZaposleniDTO>> Izmeni(
            int id,
            [FromBody] IzmeniZaposlenogDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new IzmeniZaposlenogKomanda
                {
                    Id = id,
                    Podaci = podaci
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Obrisi(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new ObrisiZaposlenogKomanda
                {
                    Id = id
                },
                cancellationToken);

            return NoContent();
        }
    }
}
