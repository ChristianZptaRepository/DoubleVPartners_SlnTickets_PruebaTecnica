using Microsoft.EntityFrameworkCore;
using Tickets.Application.Repositorio.Interface;
using Tickets.Domain.Entidades;
using Tickets.Infrastructure.Persistencia;
using static Tickets.Application.Repositorio.Interface.ITicketRepositorio;

namespace Tickets.Infrastructure.TicketRepositorio
{
    public class TicketRepositorio : ITicketRepositorio
    {
        private readonly AppDbContext _context;

        public TicketRepositorio(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Ticket> AgregarTicket(Ticket ticket)
        {
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        public async Task ActualizarTicket(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<ResultadoPagina<Ticket>> ObtenerTickets(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Ticket.CountAsync();
            var items = await _context.Ticket
                .OrderByDescending(t => t.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ResultadoPagina<Ticket>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<Ticket?> ObtenerTicketPorId(int id)
        {
            return await _context.Ticket.FindAsync(id);
        }
        public async Task<bool> EliminarTicket(int id)
        {
            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(t => t.Id == id && t.Eliminado == false);

            if (ticket == null)
            {
                return false;
            }

            ticket.MarcarComoEliminado();

            _context.Ticket.Update(ticket);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
