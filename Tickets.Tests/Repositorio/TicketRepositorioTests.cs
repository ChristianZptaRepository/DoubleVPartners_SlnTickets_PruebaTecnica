using Microsoft.EntityFrameworkCore;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;
using Tickets.Infrastructure.Persistencia;
using Tickets.Infrastructure.TicketRepositorio;
using Xunit;

namespace Tickets.Tests.Repositorio
{
    public class TicketRepositorioTests
    {
        private ITicketRepositorio CrearRepositorio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            return new TicketRepositorio(context);
        }

        public async Task AgregarTicket_DeberiaAgregarTicket()
        {
            var repo = CrearRepositorio();
            var ticket = new Ticket { Usuario = "Usuario1" };

            var agregado = await repo.AgregarTicket(ticket);

            Assert.NotNull(agregado);
            Assert.Equal("Usuario1", agregado.Usuario);
        }

        [Fact]
        public async Task ActualizarTicket_DeberiaActualizarUsuario()
        {
            var repo = CrearRepositorio();
            var ticket = await repo.AgregarTicket(new Ticket { Usuario = "Original" });

            ticket.Usuario = "Actualizado";
            await repo.ActualizarTicket(ticket);

            var actualizado = await repo.ObtenerTicketPorId(ticket.Id);
            Assert.Equal("Actualizado", actualizado.Usuario);
        }

        [Fact]
        public async Task ObtenerTicketPorId_DeberiaRetornarTicket()
        {
            var repo = CrearRepositorio();
            var ticket = await repo.AgregarTicket(new Ticket { Usuario = "Test" });

            var obtenido = await repo.ObtenerTicketPorId(ticket.Id);

            Assert.NotNull(obtenido);
            Assert.Equal("Test", obtenido.Usuario);
        }

        [Fact]
        public async Task ObtenerTickets_DeberiaRetornarPaginado()
        {
            var repo = CrearRepositorio();

            for (int i = 1; i <= 15; i++)
                await repo.AgregarTicket(new Ticket { Usuario = $"U{i}" });

            var paginado = await repo.ObtenerTickets(2, 5);

            Assert.Equal(15, paginado.TotalCount);
            Assert.Equal(5, paginado.Items.Count);
            Assert.Equal(2, paginado.PageNumber);
        }

        [Fact]
        public async Task EliminarTicket_DeberiaMarcarComoEliminado()
        {
            var repo = CrearRepositorio();
            var ticket = await repo.AgregarTicket(new Ticket { Usuario = "Eliminar" });

            var resultado = await repo.EliminarTicket(ticket.Id);
            var eliminado = await repo.ObtenerTicketPorId(ticket.Id);

            Assert.True(resultado);
            Assert.True(eliminado.Eliminado);
        }
    }
}