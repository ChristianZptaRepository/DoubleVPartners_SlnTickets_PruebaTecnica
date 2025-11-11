using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;
using static Tickets.Application.Repositorio.Interface.ITicketRepositorio;

namespace Tickets.Application.Tickets.ObtenerTickets
{
    public class ObtenerTicketsHandler : IRequestHandler<ObtenerTicketsQuery, ResultadoPagina<Ticket>>
    {
        private readonly ITicketRepositorio _ticketRepositorio;

        public ObtenerTicketsHandler(ITicketRepositorio ticketRepositorio)
        {
            _ticketRepositorio = ticketRepositorio;
        }

        public async Task<ResultadoPagina<Ticket>> Handle(ObtenerTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _ticketRepositorio.ObtenerTickets(request.PageNumber, request.PageSize);
        }
    }
}
