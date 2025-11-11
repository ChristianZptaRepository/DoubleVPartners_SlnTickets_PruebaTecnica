using MediatR;
using Tickets.Application.Repositorio.Interface;
using Tickets.Application.Tickets.Commands.ActualizarTicket;
using Tickets.Application.Tickets.Commands.AgregarTicket;
using Tickets.Application.Tickets.Commands.EliminarTicket;
using Tickets.Application.Tickets.ObtenerTicketPorId;
using Tickets.Application.Tickets.ObtenerTickets;
using Tickets.Domain.Entidades;

namespace Tickets.API.GraphQL
{    public class TicketQueries
    {
        public async Task<ResultadoPagina<Ticket>> ObtenerTickets(
            [Service] IMediator mediator, int page = 1, int pageSize = 10)
        {
            return await mediator.Send(new ObtenerTicketsQuery(page, pageSize));
        }

        public async Task<Ticket?> ObtenerTicketsPorId(
            [Service] IMediator mediator, int id)
        {
            return await mediator.Send(new ObtenerTicketPorIdQuery(id));
        }
    }
    public class TicketMutations
    {
        public async Task<Ticket> AgregarTicket(
            [Service] IMediator mediator, AgregarTicketDTO input)
        {
            return await mediator.Send(new AgregarTicketCommand(input.Usuario));
        }

        public async Task<Ticket?> ActualizarTicket(
            [Service] IMediator mediator, int id, ActualizarTicketDTO input)
        {
            return await mediator.Send(new ActualizarTicketCommand(id, input.Usuario, input.Estatus));
        }
        public async Task<bool> EliminarTicket(int id,[Service] IMediator mediator)
        {
            var command = new EliminarTicketCommand(id);

            var result = await mediator.Send(command);

            return result;
        }
    }
}
