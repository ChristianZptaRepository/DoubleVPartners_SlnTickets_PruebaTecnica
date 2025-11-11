//using HotChocolate;
//using HotChocolate.Execution;
//using MediatR;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using Tickets.API.GraphQL;
//using Tickets.Application.Repositorio.Interface;
//using Tickets.Application.Tickets.Commands.ActualizarTicket;
//using Tickets.Application.Tickets.Commands.AgregarTicket;
//using Tickets.Application.Tickets.Commands.EliminarTicket;
//using Tickets.Application.Tickets.ObtenerTicketPorId;
//using Tickets.Application.Tickets.ObtenerTickets;
//using Tickets.Domain.Entidades;
//using Tickets.Domain.Enums;

//namespace Tickets.Tests.GraphQL
//{
//    public class TicketGraphQLTests
//    {
//        private async Task<IRequestExecutor> CrearExecutor(Mock<IMediator> mediatorMock)
//        {
//            var services = new ServiceCollection();
//            services.AddSingleton<IMediator>(mediatorMock.Object);

//            services.AddGraphQL()
//                    .AddQueryType<TicketQueries>()
//                    .AddMutationType<TicketMutations>();

//            var sp = services.BuildServiceProvider();
//            return await sp
//                .GetRequiredService<IRequestExecutorResolver>()
//                .GetRequestExecutorAsync();
//        }

//        [Fact]
//        public async Task Query_ObtenerTickets_DeberiaRetornarLista()
//        {
//            var ticketsList = new List<Ticket>
//            {
//                new Ticket { Id = 1, Usuario = "U1" },
//                new Ticket { Id = 2, Usuario = "U2" }
//            };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTicketsQuery>(), default))
//                        .ReturnsAsync(new ResultadoPagina<Ticket>
//                        {
//                            Items = ticketsList,
//                            TotalCount = 2,
//                            PageNumber = 1,
//                            PageSize = 10
//                        });

//            var executor = await CrearExecutor(mediatorMock);

//            var query = "{ obtenerTickets { items { id usuario } totalCount } }";
//            var result = await executor.ExecuteAsync(query);

//            Assert.Null(result.Errors);

//            var ticketsData = result.Data!.GetProperty<IReadOnlyList<Dictionary<string, object>>>("obtenerTickets")!;
//            Assert.Equal(2, ticketsData.Count);
//            Assert.Equal("U1", ticketsData[0]["usuario"]);
//        }

//        [Fact]
//        public async Task Query_ObtenerTicketsPorId_DeberiaRetornarTicket()
//        {
//            var ticket = new Ticket { Id = 1, Usuario = "User1" };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTicketPorIdQuery>(), default))
//                        .ReturnsAsync(ticket);

//            var executor = await CrearExecutor(mediatorMock);

//            var query = "{ obtenerTicketsPorId(id:1) { id usuario } }";
//            var result = await executor.ExecuteAsync(query);

//            Assert.Null(result.Errors);

//            var ticketData = result.Data!.GetProperty<Dictionary<string, object>>("obtenerTicketsPorId")!;
//            Assert.Equal(1, ticketData["id"]);
//            Assert.Equal("User1", ticketData["usuario"]);
//        }

//        [Fact]
//        public async Task Mutation_AgregarTicket_DeberiaRetornarTicket()
//        {
//            var ticket = new Ticket { Id = 1, Usuario = "Nuevo" };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<AgregarTicketCommand>(), default))
//                        .ReturnsAsync(ticket);

//            var executor = await CrearExecutor(mediatorMock);

//            var mutation = @"
//            mutation {
//                agregarTicket(input: { usuario: ""Nuevo"" }) { id usuario }
//            }";

//            var result = await executor.ExecuteAsync(mutation);
//            Assert.Null(result.Errors);

//            var ticketData = result.Data!.GetProperty<Dictionary<string, object>>("agregarTicket")!;
//            Assert.Equal(1, ticketData["id"]);
//            Assert.Equal("Nuevo", ticketData["usuario"]);
//        }

//        [Fact]
//        public async Task Mutation_ActualizarTicket_DeberiaRetornarTicket()
//        {
//            var ticket = new Ticket { Id = 1, Usuario = "User1", Estatus = EstadoTicket.Cerrado };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<ActualizarTicketCommand>(), default))
//                        .ReturnsAsync(ticket);

//            var executor = await CrearExecutor(mediatorMock);

//            var mutation = @"
//            mutation {
//                actualizarTicket(id:1, input:{ usuario:""User1"", estatus:""Cerrado"" }) {
//                    id
//                    usuario
//                    estatus
//                }
//            }";

//            var result = await executor.ExecuteAsync(mutation);
//            Assert.Null(result.Errors);

//            var ticketData = result.Data!.GetProperty<Dictionary<string, object>>("actualizarTicket")!;
//            Assert.Equal(1, ticketData["id"]);
//            Assert.Equal("User1", ticketData["usuario"]);
//            Assert.Equal("Cerrado", ticketData["estatus"]);
//        }

//        [Fact]
//        public async Task Mutation_EliminarTicket_DeberiaRetornarTrue()
//        {
//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<EliminarTicketCommand>(), default))
//                        .ReturnsAsync(true);

//            var executor = await CrearExecutor(mediatorMock);

//            var mutation = "mutation { eliminarTicket(id:1) }";
//            var result = await executor.ExecuteAsync(mutation);
//            Assert.Null(result.Errors);

//            var eliminado = result.Data!.GetProperty<bool>("eliminarTicket");
//            Assert.True(eliminado);
//        }

//        [Fact]
//        public async Task Mutation_EliminarTicket_Fallo_DeberiaRetornarFalse()
//        {
//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(m => m.Send(It.IsAny<EliminarTicketCommand>(), default))
//                        .ReturnsAsync(false);

//            var executor = await CrearExecutor(mediatorMock);

//            var mutation = "mutation { eliminarTicket(id:1) }";
//            var result = await executor.ExecuteAsync(mutation);
//            Assert.Null(result.Errors);

//            var eliminado = result.Data!.GetProperty<bool>("eliminarTicket");
//            Assert.False(eliminado);
//        }
//    }
//}
