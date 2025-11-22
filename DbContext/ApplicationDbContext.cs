using Microsoft.EntityFrameworkCore;
using family_tree_builder.Models;

namespace family_tree_builder.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions for dependency injection
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DB table
        public DbSet<PersonNode> PersonNodes { get; set; } = null!;
    }
}
