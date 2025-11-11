using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entidades;
using Tickets.Domain.Enums;

namespace Tickets.Application.Tickets.Commands.ActualizarTicket
{
    public record ActualizarTicketCommand(int Id, string Usuario, EstadoTicket Estatus) : IRequest<Ticket?>;
}
