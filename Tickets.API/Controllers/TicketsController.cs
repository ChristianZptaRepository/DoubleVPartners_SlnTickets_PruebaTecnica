using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.Repositorio.Interface;
using Tickets.Application.Tickets.Commands.ActualizarTicket;
using Tickets.Application.Tickets.Commands.AgregarTicket;
using Tickets.Application.Tickets.Commands.EliminarTicket;
using Tickets.Application.Tickets.ObtenerTicketPorId;
using Tickets.Application.Tickets.ObtenerTickets;

namespace Tickets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTickets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ObtenerTicketsQuery(page, pageSize);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> ObtenerTicketPorId(int Id)
        {
            var query = new ObtenerTicketPorIdQuery(Id);
            var ticket = await _mediator.Send(query);
            return ticket == null ? NotFound() : Ok(ticket);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTicket(AgregarTicketDTO dto)
        {
            var command = new AgregarTicketCommand(dto.Usuario);
            var ticket = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObtenerTicketPorId), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizarTicketDTO dto)
        {
            var command = new ActualizarTicketCommand(id, dto.Usuario, dto.Estatus);
            var ticket = await _mediator.Send(command);
            return ticket == null ? NotFound() : Ok(ticket);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTicket(int id)
        {
            var command = new EliminarTicketCommand(id);

            var eliminado = await _mediator.Send(command);

            if (!eliminado)
            {
                return NotFound($"Ticket con ID {id} no encontrado.");
            }
            return NoContent();
        }
    }
}
