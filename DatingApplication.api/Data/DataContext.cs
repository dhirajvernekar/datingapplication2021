using DatingApplication.api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Value> Values { get;set; }
    }
}