using Microsoft.EntityFrameworkCore;
using YouYou.Api.Models;

namespace YouYou.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<Usuario> Usuarios{ get; set; }
    }
}
