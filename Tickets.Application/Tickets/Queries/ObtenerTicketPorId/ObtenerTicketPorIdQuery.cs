using MediatR;
using Tickets.Domain.Entidades;

namespace Tickets.Application.Tickets.ObtenerTicketPorId
{
    public record ObtenerTicketPorIdQuery(int Id) : IRequest<Ticket>;
}
