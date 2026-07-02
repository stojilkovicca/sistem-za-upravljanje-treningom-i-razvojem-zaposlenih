using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.KategorijePrograma;
using TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Brisanje;
using TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Izmena;
using TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Komande.Kreiranje;
using TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiPoId;
using TreningIRazvoj.API.Funkcionalnosti.KategorijePrograma.Upiti.VratiSve;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/kategorije-programa")]
    [Authorize]
    public class KategorijeProgramaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KategorijeProgramaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<KategorijaProgramaDTO>>> VratiSve(
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiSveKategorijeProgramaUpit(),
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<KategorijaProgramaDTO>> VratiPoId(
            int id,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiKategorijuProgramaPoIdUpit
                {
                    Id = id
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPost]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<KategorijaProgramaDTO>> Kreiraj(
            [FromBody] KreirajKategorijuProgramaDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new KreirajKategorijuProgramaKomanda
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
        public async Task<ActionResult<KategorijaProgramaDTO>> Izmeni(
            int id,
            [FromBody] IzmeniKategorijuProgramaDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new IzmeniKategorijuProgramaKomanda
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
                new ObrisiKategorijuProgramaKomanda
                {
                    Id = id
                },
                cancellationToken);

            return NoContent();
        }
    }
}