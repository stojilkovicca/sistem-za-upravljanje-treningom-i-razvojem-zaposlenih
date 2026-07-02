using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.RazvojniProgrami;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Brisanje;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Izmena;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Komande.Kreiranje;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiPoId;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.VratiSve;
using TreningIRazvoj.API.DTO.Opste;
using TreningIRazvoj.API.Funkcionalnosti.RazvojniProgrami.Upiti.Pretraga;
using TreningIRazvoj.Domen.Enumeracije;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/razvojni-programi")]
    [Authorize]
    public class RazvojniProgramiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RazvojniProgramiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RazvojniProgramDTO>>> VratiSve(
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiSveRazvojneProgrameUpit(),
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("pretraga")]
        public async Task<ActionResult<StranicniRezultat<RazvojniProgramDTO>>> Pretrazi(
            [FromQuery] string? pretraga,
            [FromQuery] VrstaPrograma? vrsta,
            [FromQuery] int? kategorijaProgramaId,
            [FromQuery] int? predavacId,
            [FromQuery] bool? objavljen,
            [FromQuery] string sortirajPo = "datumPocetka",
            [FromQuery] string smerSortiranja = "rastuce",
            [FromQuery] int brojStranice = 1,
            [FromQuery] int velicinaStranice = 10,
            CancellationToken cancellationToken = default)
        {
            var rezultat = await _mediator.Send(
                new PretraziRazvojneProgrameUpit
                {
                    Pretraga = pretraga,
                    Vrsta = vrsta,
                    KategorijaProgramaId = kategorijaProgramaId,
                    PredavacId = predavacId,
                    Objavljen = objavljen,
                    SortirajPo = sortirajPo,
                    SmerSortiranja = smerSortiranja,
                    BrojStranice = brojStranice,
                    VelicinaStranice = velicinaStranice
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RazvojniProgramDTO>> VratiPoId(
            int id,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiRazvojniProgramPoIdUpit
                {
                    Id = id
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPost]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<RazvojniProgramDTO>> Kreiraj(
            [FromBody] KreirajRazvojniProgramDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new KreirajRazvojniProgramKomanda
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
        public async Task<ActionResult<RazvojniProgramDTO>> Izmeni(
            int id,
            [FromBody] IzmeniRazvojniProgramDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new IzmeniRazvojniProgramKomanda
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
                new ObrisiRazvojniProgramKomanda
                {
                    Id = id
                },
                cancellationToken);

            return NoContent();
        }
    }
}