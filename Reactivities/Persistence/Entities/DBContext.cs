using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Entities;

public class DBContext : IdentityDbContext<AppUser>
{
    public DBContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
}