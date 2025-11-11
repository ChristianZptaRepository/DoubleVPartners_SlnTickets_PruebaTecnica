using Tickets.Domain.Enums;

namespace Tickets.Domain.Entidades
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public EstadoTicket Estatus { get; set; }
        public bool Eliminado { get; private set; } = false;
        public void MarcarComoEliminado()
        {
            Eliminado = true;
        }
    }
}
