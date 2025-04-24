using Microsoft.EntityFrameworkCore;
using LoncotesCountyLib.Models;

public class LoncotesCountyLibDbContext : DbContext
{

    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }

    public LoncotesCountyLibDbContext(DbContextOptions<LoncotesCountyLibDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patron>().HasData(new Patron[]
        {
            new Patron {Id = 1, FirstName = "Tom", LastName = "Perkins", Address = "123 Main St.", Email = "tom.perkins@mail.com", IsActive = true},
            new Patron {Id = 2, FirstName = "Steve", LastName = "Oh", Address = "134 South St.", Email = "steve.oh@mail.com", IsActive = true} 
        });

        modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
        {
            new MaterialType {Id = 1, Name = "Book", CheckoutDays = 7},
            // ! Need to add  2 more MaterialType
        });

        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            // ! Need to add 5 Genres
        });

        modelBuilder.Entity<Material>().HasData(new Material[]
        {
            // ! Need to add 10 Materials
        });
        
    }

}