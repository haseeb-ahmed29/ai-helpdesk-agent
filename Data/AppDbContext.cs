using Microsoft.EntityFrameworkCore;
using AiHelpdeskAgent.Models;

namespace AiHelpdeskAgent.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
}
