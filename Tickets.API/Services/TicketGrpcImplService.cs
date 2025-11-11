using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using System.Net.Sockets;
using Tickets.Application.Repositorio.Interface;
using Tickets.Application.Tickets.Commands.AgregarTicket;
using Tickets.Application.Tickets.Commands.EliminarTicket;
using Tickets.Application.Tickets.ObtenerTicketPorId;
using Tickets.Domain.Entidades;
using Tickets.Domain.Enums;

namespace Tickets.API
{
    public class TicketGrpcImplService : TicketProtoService.TicketProtoServiceBase
    {
        private readonly IMediator _mediator;
        public TicketGrpcImplService(IMediator mediator) => _mediator = mediator;

        public override async Task<TicketResponse> ObtenerTicket(ObtenerTicketRequest request, ServerCallContext context)
        {
            var query = new ObtenerTicketPorIdQuery(request.Id);
            var ticket = await _mediator.Send(query);

            if (ticket == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

            return MapToResponse(ticket);
        }
        public override async Task<ListarTicketsResponse> ListarTickets(ListarTicketsRequest request, ServerCallContext context)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var query = new Tickets.Application.Tickets.ObtenerTickets.ObtenerTicketsQuery(
                pageNumber,
                pageSize
            );

            var pagedResult = (ResultadoPagina<Ticket>)await _mediator.Send(query);

            var domainTicketsList = pagedResult.Items;

            var response = new ListarTicketsResponse();

            response.Tickets.AddRange(domainTicketsList.Select(MapToResponse));

            return response;
        }
        public override async Task<TicketResponse> ActualizarTicket(ActualizarTicketRequest request, ServerCallContext context)
        {
            if (!System.Enum.TryParse<EstadoTicket>(request.Estatus, true, out var nuevoEstado))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Estatus '{request.Estatus}' no es válido."));
            }

            var command = new Tickets.Application.Tickets.Commands.ActualizarTicket.ActualizarTicketCommand(
                request.Id,
                request.Usuario,
                nuevoEstado
            );

            var ticket = await _mediator.Send(command);

            if (ticket == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Ticket a actualizar no encontrado."));

            return MapToResponse(ticket);
        }
        public override async Task<TicketResponse> AgregarTicket(AgregarTicketRequest request, ServerCallContext context)
        {
            var command = new AgregarTicketCommand(request.Usuario);
            var ticket = await _mediator.Send(command);
            return MapToResponse(ticket);
        }

        public override async Task<EliminarTicketResponse> EliminarTicket(EliminarTicketRequest request,ServerCallContext context)
        {
            var command = new EliminarTicketCommand(request.Id);

            var eliminado = await _mediator.Send(command);

            if (!eliminado)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Ticket con ID {request.Id} no encontrado o ya estaba eliminado."));
            }

            return new EliminarTicketResponse { Success = true };
        }
        private TicketResponse MapToResponse(Domain.Entidades.Ticket ticket) => new TicketResponse
        {
            Id = ticket.Id,
            Usuario = ticket.Usuario,
            Estatus = ticket.Estatus.ToString(),
            FechaCreacion = Timestamp.FromDateTime(ticket.FechaCreacion.ToUniversalTime())
        };
    }
}
