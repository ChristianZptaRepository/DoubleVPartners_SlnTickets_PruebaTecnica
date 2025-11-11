using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entidades;

namespace Tickets.Infrastructure.Persistencia
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Ticket> Ticket { get; set; }
    }
}
