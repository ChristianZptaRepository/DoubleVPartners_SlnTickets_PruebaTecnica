using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Repositorio.Interface;

namespace Tickets.Application.Tickets.Commands.EliminarTicket
{
    public class EliminarTicketHandler : IRequestHandler<EliminarTicketCommand, bool>
    {
        private readonly ITicketRepositorio _ticketRepositorio;

        public EliminarTicketHandler(ITicketRepositorio ticketRepositorio) => _ticketRepositorio = ticketRepositorio;

        public async Task<bool> Handle(EliminarTicketCommand request, CancellationToken cancellationToken)
        {
            var result = await _ticketRepositorio.EliminarTicket(request.Id);

            return result;
        }
    }
}
