using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entidades;

namespace Tickets.Application.Tickets.Commands.AgregarTicket
{
    public record AgregarTicketCommand(string Usuario) : IRequest<Ticket>;
}
