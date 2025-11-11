using MediatR;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;

namespace Tickets.Application.Tickets.ObtenerTicketPorId
{
    public class ObtenerTicketPorIdHandler : IRequestHandler<ObtenerTicketPorIdQuery, Ticket?>
    {
        private readonly ITicketRepositorio _ticketRepositorio;

        public ObtenerTicketPorIdHandler(ITicketRepositorio ticketRepositorio)
        {
            _ticketRepositorio = ticketRepositorio;
        }

        public async Task<Ticket?> Handle(ObtenerTicketPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _ticketRepositorio.ObtenerTicketPorId(request.Id);
        }
    }
}
