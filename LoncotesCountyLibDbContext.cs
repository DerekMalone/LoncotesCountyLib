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
            new MaterialType {Id = 2, Name = "Newspaper", CheckoutDays = 3},
            new MaterialType {Id = 3, Name = "CD", CheckoutDays = 5},
            new MaterialType {Id = 4, Name = "8Track", CheckoutDays = 5}
        });

        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            new Genre {Id = 1, Name = "Scy-Fi"},
            new Genre {Id = 2, Name = "Biography"},
            new Genre {Id = 3, Name = "Fantasy"},
            new Genre {Id = 4, Name = "Thriller"},
            new Genre {Id = 5, Name = "Cooking"}
        });

        modelBuilder.Entity<Material>().HasData(new Material[]
        {
            new Material {Id = 1, MaterialName = "Lord of the Rings", MaterialTypeId = 1, GenreId = 3, OutOfCirculationSince = new DateTime(2016, 05, 01)},
            new Material {Id = 2, MaterialName = "The NY Times", MaterialTypeId = 2, GenreId = 5, OutOfCirculationSince = new DateTime(2020, 07, 12)},
            new Material {Id = 3, MaterialName = "Abraham Lincoln, Vampire Slayer", MaterialTypeId = 1, GenreId = 2, OutOfCirculationSince = new DateTime(2000, 01, 29)},
            new Material {Id = 4, MaterialName = "Sherlock Holmes", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = new DateTime(2015, 09, 01)},
            new Material {Id = 5, MaterialName = "Bethovens 5th", MaterialTypeId = 3, GenreId = 4, OutOfCirculationSince = new DateTime(1989, 06, 16)},
            new Material {Id = 6, MaterialName = "Isaac Hayes", MaterialTypeId = 4, GenreId = 1, OutOfCirculationSince = new DateTime(1997, 05, 05)},
            new Material {Id = 7, MaterialName = "The Expanse", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = new DateTime(2015, 04, 02)},
            new Material {Id = 8, MaterialName = "Dungeon Crawler Carl", MaterialTypeId = 1, GenreId = 3, OutOfCirculationSince = new DateTime(2017, 11, 11)},
            new Material {Id = 9, MaterialName = "Good Charlotte", MaterialTypeId = 3, GenreId = 3, OutOfCirculationSince = new DateTime(2001, 12, 03)},
            new Material {Id = 10, MaterialName = "Paleo Diet", MaterialTypeId = 4, GenreId = 5, OutOfCirculationSince = new DateTime(2016, 08, 19)}
        });
        
    }

}