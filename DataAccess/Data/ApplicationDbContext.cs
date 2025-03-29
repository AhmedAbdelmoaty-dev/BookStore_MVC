using Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options):base(Options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Category>().HasData(
                new Category() { ID = 1, Name = "Action", DisplayOrder = 1 },
                new Category() { ID = 2, Name = "Sci-Fi", DisplayOrder = 2 },
                new Category() { ID = 3, Name = "History", DisplayOrder = 3 }
                );
        }
        public DbSet<Category>Categories { get; set; }
        
    }
}
