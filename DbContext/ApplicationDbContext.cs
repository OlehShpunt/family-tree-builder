using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;
using family_tree_builder.Models;

namespace family_tree_builder.Data
{
    // Change from DbContext → IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonNode> PersonNodes { get; set; } = null!;
    }
}