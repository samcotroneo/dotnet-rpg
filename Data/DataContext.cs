using dotnet_rpg.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    // An instance of DbContext, represents a session with the database (Queries, Save etc).
    // Combination of the Unity of Work, and Repository Patterns.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
        : base(options)
        {

        }

        public DbSet<Character> Characters { get; set; }
    }
}