using JD.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JD.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}