using Microsoft.EntityFrameworkCore;
using WebAPI_tutorial.Entities;

namespace WebAPI_tutorial.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    }
}
