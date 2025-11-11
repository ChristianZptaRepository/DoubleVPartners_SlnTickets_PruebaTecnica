using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;
using Tickets.Domain.Enums;

namespace Tickets.Application.Tickets.Commands.AgregarTicket
{
    internal class AgregarTicketHandler : IRequestHandler<AgregarTicketCommand, Ticket>
    {
        private readonly ITicketRepositorio _ticketRepositorio;

        public AgregarTicketHandler(ITicketRepositorio ticketRepositorio)
        {
            _ticketRepositorio = ticketRepositorio;
        }

        public async Task<Ticket> Handle(AgregarTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket
            {
                Usuario = request.Usuario,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                Estatus = EstadoTicket.Abierto
            };

            return await _ticketRepositorio.AgregarTicket(ticket);
        }
    }
}
