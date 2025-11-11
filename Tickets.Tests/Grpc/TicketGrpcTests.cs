using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tickets.API;
using Tickets.Application.Repositorio.Interface;
using Tickets.Application.Tickets.Commands.ActualizarTicket;
using Tickets.Application.Tickets.Commands.AgregarTicket;
using Tickets.Application.Tickets.Commands.EliminarTicket;
using Tickets.Application.Tickets.ObtenerTicketPorId;
using Tickets.Application.Tickets.ObtenerTickets;
using Tickets.Domain.Entidades;
using Tickets.Domain.Enums;
using Tickets.Infrastructure.Persistencia;
using Tickets.Infrastructure.TicketRepositorio;

namespace Tickets.Tests.Grpc
{
    public class TicketRepositorioTests
    {
        private TicketGrpcImplService CrearService(Mock<IMediator> mediatorMock)
            => new TicketGrpcImplService(mediatorMock.Object);

        [Fact]
        public async Task ObtenerTicket_DeberiaRetornarTicket()
        {
            var ticket = new Ticket { Id = 1, Usuario = "User1", Estatus = EstadoTicket.Abierto };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTicketPorIdQuery>(), default))
                        .ReturnsAsync(ticket);

            var service = CrearService(mediatorMock);

            var response = await service.ObtenerTicket(new Tickets.ObtenerTicketRequest { Id = 1 }, null);

            Assert.NotNull(response);
            Assert.Equal("User1", response.Usuario);
            Assert.Equal(1, response.Id);
        }

        [Fact]
        public async Task ObtenerTicket_NoEncontrado_DeberiaLanzarRpcException()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTicketPorIdQuery>(), default))
                        .ReturnsAsync((Ticket)null);

            var service = CrearService(mediatorMock);

            await Assert.ThrowsAsync<RpcException>(() =>
                service.ObtenerTicket(new Tickets.ObtenerTicketRequest { Id = 1 }, null));
        }

        [Fact]
        public async Task AgregarTicket_DeberiaRetornarTicket()
        {
            var ticket = new Ticket { Id = 1, Usuario = "Nuevo", Estatus = EstadoTicket.Abierto };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<AgregarTicketCommand>(), default))
                        .ReturnsAsync(ticket);

            var service = CrearService(mediatorMock);

            var response = await service.AgregarTicket(new Tickets.AgregarTicketRequest { Usuario = "Nuevo" }, null);

            Assert.NotNull(response);
            Assert.Equal("Nuevo", response.Usuario);
        }

        [Fact]
        public async Task ActualizarTicket_DeberiaRetornarTicket()
        {
            var ticket = new Ticket { Id = 1, Usuario = "User1", Estatus = EstadoTicket.Cerrado };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ActualizarTicketCommand>(), default))
                        .ReturnsAsync(ticket);

            var service = CrearService(mediatorMock);

            var request = new Tickets.ActualizarTicketRequest
            {
                Id = 1,
                Usuario = "User1",
                Estatus = "Cerrado"
            };

            var response = await service.ActualizarTicket(request, null);

            Assert.NotNull(response);
            Assert.Equal("Cerrado", response.Estatus);
        }

        [Fact]
        public async Task ActualizarTicket_EstatusInvalido_DeberiaLanzarRpcException()
        {
            var mediatorMock = new Mock<IMediator>();
            var service = CrearService(mediatorMock);

            var request = new Tickets.ActualizarTicketRequest
            {
                Id = 1,
                Usuario = "User1",
                Estatus = "NoValido"
            };

            await Assert.ThrowsAsync<RpcException>(() =>
                service.ActualizarTicket(request, null));
        }

        [Fact]
        public async Task EliminarTicket_DeberiaRetornarSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<EliminarTicketCommand>(), default))
                        .ReturnsAsync(true);

            var service = CrearService(mediatorMock);

            var response = await service.EliminarTicket(new Tickets.EliminarTicketRequest { Id = 1 }, null);

            Assert.True(response.Success);
        }

        [Fact]
        public async Task EliminarTicket_NoEncontrado_DeberiaLanzarRpcException()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<EliminarTicketCommand>(), default))
                        .ReturnsAsync(false);

            var service = CrearService(mediatorMock);

            await Assert.ThrowsAsync<RpcException>(() =>
                service.EliminarTicket(new Tickets.EliminarTicketRequest { Id = 1 }, null));
        }

        [Fact]
        public async Task ListarTickets_DeberiaRetornarLista()
        {
            var ticketsList = new List<Ticket>
            {
                new Ticket { Id = 1, Usuario = "U1", Estatus = EstadoTicket.Abierto },
                new Ticket { Id = 2, Usuario = "U2", Estatus = EstadoTicket.Cerrado }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTicketsQuery>(), default))
                        .ReturnsAsync(new ResultadoPagina<Ticket>
                        {
                            Items = ticketsList,
                            TotalCount = 2,
                            PageNumber = 1,
                            PageSize = 10
                        });

            var service = CrearService(mediatorMock);

            var response = await service.ListarTickets(new Tickets.ListarTicketsRequest
            {
                PageNumber = 1,
                PageSize = 10
            }, null);

            Assert.NotNull(response);
            Assert.Equal(2, response.Tickets.Count);
            Assert.Equal("U1", response.Tickets[0].Usuario);
        }
    }
}
