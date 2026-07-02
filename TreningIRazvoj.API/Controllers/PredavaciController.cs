using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.Predavaci;
using TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Brisanje;
using TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Izmena;
using TreningIRazvoj.API.Funkcionalnosti.Predavaci.Komande.Kreiranje;
using TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiPoId;
using TreningIRazvoj.API.Funkcionalnosti.Predavaci.Upiti.VratiSve;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/predavaci")]
    [Authorize]
    public class PredavaciController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PredavaciController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredavacDTO>>> VratiSve(
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiSvePredavaceUpit(),
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PredavacDTO>> VratiPoId(
            int id,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiPredavacaPoIdUpit
                {
                    Id = id
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPost]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<PredavacDTO>> Kreiraj(
            [FromBody] KreirajPredavacaDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new KreirajPredavacaKomanda
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
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<PredavacDTO>> Izmeni(
            int id,
            [FromBody] IzmeniPredavacaDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new IzmeniPredavacaKomanda
                {
                    Id = id,
                    Podaci = podaci
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<IActionResult> Obrisi(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new ObrisiPredavacaKomanda
                {
                    Id = id
                },
                cancellationToken);

            return NoContent();
        }
    }
}