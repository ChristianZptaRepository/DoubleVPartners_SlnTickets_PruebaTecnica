using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;

namespace Tickets.Application.Tickets.ObtenerTickets
{
    public record ObtenerTicketsQuery(int PageNumber, int PageSize) : IRequest<ResultadoPagina<Ticket>>;
}
