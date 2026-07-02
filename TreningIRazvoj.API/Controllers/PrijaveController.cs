using Microsoft.AspNetCore.Authorization;
using TreningIRazvoj.API.Autorizacija;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreningIRazvoj.API.DTO.Prijave;
using TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Kreiranje;
using TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Obrada;
using TreningIRazvoj.API.Funkcionalnosti.Prijave.Komande.Ocenjivanje;
using TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiPoId;
using TreningIRazvoj.API.Funkcionalnosti.Prijave.Upiti.VratiSve;
using TreningIRazvoj.API.Servisi;

namespace TreningIRazvoj.API.Controllers
{
    [ApiController]
    [Route("api/prijave")]
    [Authorize]
    public class PrijaveController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIzvestajServis _izvestajServis;

        public PrijaveController(
            IMediator mediator,
            IIzvestajServis izvestajServis)
        {
            _mediator = mediator;
            _izvestajServis = izvestajServis;
        }

        [HttpGet]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<IEnumerable<PrijavaDTO>>> VratiSve(
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiSvePrijaveUpit(),
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpGet("izvestaj")]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<IActionResult> PreuzmiIzvestaj(
            CancellationToken cancellationToken)
        {
            var prijave = await _mediator.Send(
                new VratiSvePrijaveUpit(),
                cancellationToken);

            var pdf = _izvestajServis
                .GenerisiIzvestajOPrijavama(prijave);

            var nazivFajla =
                $"izvestaj-prijave-{DateTime.Now:yyyy-MM-dd-HH-mm}.pdf";

            return File(
                pdf,
                "application/pdf",
                nazivFajla);
        }

        



        [HttpGet("{zaposleniId:int}/{razvojniProgramId:int}")]
        [Authorize(
            Roles = Uloge.Administrator + "," + Uloge.HR + "," + Uloge.Zaposleni)]
        public async Task<ActionResult<PrijavaDTO>> VratiPoId(
            int zaposleniId,
            int razvojniProgramId,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new VratiPrijavuPoIdUpit
                {
                    ZaposleniId = zaposleniId,
                    RazvojniProgramId = razvojniProgramId
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPost]
        [Authorize(
            Roles = Uloge.Administrator + "," + Uloge.HR + "," + Uloge.Zaposleni)]
        public async Task<ActionResult<PrijavaDTO>> Kreiraj(
            [FromBody] KreirajPrijavuDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new KreirajPrijavuKomanda
                {
                    Podaci = podaci
                },
                cancellationToken);

            return CreatedAtAction(
                nameof(VratiPoId),
                new
                {
                    zaposleniId = rezultat.ZaposleniId,
                    razvojniProgramId = rezultat.RazvojniProgramId
                },
                rezultat);
        }

        [HttpPut("{zaposleniId:int}/{razvojniProgramId:int}/obrada")]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.HR)]
        public async Task<ActionResult<PrijavaDTO>> Obradi(
            int zaposleniId,
            int razvojniProgramId,
            [FromBody] ObradiPrijavuDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new ObradiPrijavuKomanda
                {
                    ZaposleniId = zaposleniId,
                    RazvojniProgramId = razvojniProgramId,
                    Podaci = podaci
                },
                cancellationToken);

            return Ok(rezultat);
        }

        [HttpPut("{zaposleniId:int}/{razvojniProgramId:int}/ocena")]
        [Authorize(Roles = Uloge.Administrator + "," + Uloge.Zaposleni)]
        public async Task<ActionResult<PrijavaDTO>> Oceni(
            int zaposleniId,
            int razvojniProgramId,
            [FromBody] OceniProgramDTO podaci,
            CancellationToken cancellationToken)
        {
            var rezultat = await _mediator.Send(
                new OceniProgramKomanda
                {
                    ZaposleniId = zaposleniId,
                    RazvojniProgramId = razvojniProgramId,
                    Podaci = podaci
                },
                cancellationToken);

            return Ok(rezultat);
        }
    }
}