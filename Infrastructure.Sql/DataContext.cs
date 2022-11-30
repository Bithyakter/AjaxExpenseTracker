using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> o) : base(o)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}