using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Application.Tickets.Commands.EliminarTicket
{
    public record EliminarTicketCommand(int Id) : IRequest<bool>;
}
