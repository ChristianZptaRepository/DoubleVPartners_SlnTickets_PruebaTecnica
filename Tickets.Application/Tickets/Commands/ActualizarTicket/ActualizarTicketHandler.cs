using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;

namespace Tickets.Application.Tickets.Commands.ActualizarTicket
{
    internal class ActualizarTicketHandler : IRequestHandler<ActualizarTicketCommand, Ticket?>
    {
        private readonly ITicketRepositorio _ticketRepositorio;

        public ActualizarTicketHandler(ITicketRepositorio ticketRepositorio)
        {
            _ticketRepositorio = ticketRepositorio;
        }

        public async Task<Ticket?> Handle(ActualizarTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepositorio.ObtenerTicketPorId(request.Id);
            if (ticket == null) return null;

            ticket.Usuario = request.Usuario;
            ticket.Estatus = request.Estatus;
            ticket.FechaActualizacion = DateTime.UtcNow;

            await _ticketRepositorio.ActualizarTicket(ticket);
            return ticket;
        }
    }
}
