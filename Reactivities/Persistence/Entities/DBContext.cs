using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Entities;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Activity> Activities { get; set; }
}