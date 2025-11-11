using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entidades;
using Tickets.Domain.Enums;

namespace Tickets.Application.Repositorio.Interface
{
    public interface ITicketRepositorio
    {
        Task ActualizarTicket(Ticket ticket);
        Task<Ticket> AgregarTicket(Ticket ticket);
        Task<bool> EliminarTicket(int id);
        Task<Ticket?> ObtenerTicketPorId(int id);
        Task<ResultadoPagina<Ticket>> ObtenerTickets(int pageNumber, int pageSize);
        
    }
    public class ResultadoPagina<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public record AgregarTicketDTO(string Usuario);
    public record ActualizarTicketDTO(string Usuario, EstadoTicket Estatus);
}
