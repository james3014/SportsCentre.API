using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class DataContext : DbContext
    {   
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public DbSet<Value> Values { get; set; }
    }
}