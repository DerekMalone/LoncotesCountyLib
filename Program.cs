using LoncotesCountyLib.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using LoncotesCountyLib.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LoncotesCountyLibDbContext>(builder.Configuration["LoncotesCountyLibDbConnectionString"]);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/materials", (LoncotesCountyLibDbContext db, int? materialTypeId, int? genreId) => 
{
    var filteredMaterials = db.Materials
    .Include(m => m.MaterialType)
    .Include(m => m.Genre)
    .Where(m => m.OutOfCirculationSince == null);

    if (materialTypeId != null)
    {
        filteredMaterials = filteredMaterials.Where(m => m.MaterialTypeId == materialTypeId);
    }
    if (genreId != null)
    {
        filteredMaterials = filteredMaterials.Where(m => m.GenreId == genreId);
    }

    var materials = filteredMaterials
    .Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        GenreId = m.GenreId,
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        },
        OutOfCirculationSince = m.OutOfCirculationSince
    }).ToList();

    if (materials.Count < 1)
    {
        return Results.NotFound();
    }
    return Results.Ok(materials);
});

app.MapGet("/api/materials/{id}", (LoncotesCountyLibDbContext db, int id) => 
{
    var material = db.Materials.Where(m => m.Id == id)
    .Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        GenreId = m.GenreId,
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        },
        OutOfCirculationSince = m.OutOfCirculationSince
    }).SingleOrDefault();

    if (material != null)
    {
    return Results.Ok(material);
    }

    return Results.NotFound();
});

app.MapPost("/api/materials", (LoncotesCountyLibDbContext db, Material material) =>
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

app.MapPut("/api/materials/{id}/delete", (LoncotesCountyLibDbContext db, int id) => 
{
    Material materialToUpdate = db.Materials.SingleOrDefault(m => m.Id == id);
    
    if (materialToUpdate == null)
    {
    return Results.NotFound();
    }

    materialToUpdate.OutOfCirculationSince = DateTime.Now;

    db.SaveChanges();
    return Results.NoContent();
    
});

app.MapGet("/api/materialTypes", (LoncotesCountyLibDbContext db) => 
{
    var materialTypes = db.MaterialTypes.Select(mt => new MaterialTypeDTO
    {
        Id = mt.Id,
        Name = mt.Name,
        CheckoutDays = mt.CheckoutDays
    }).ToList();

    if (materialTypes == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(materialTypes);
});

app.MapGet("/api/genres", (LoncotesCountyLibDbContext db) => 
{
    var genres = db.Genres.Select(g => new GenreDTO
    {
        Id = g.Id,
        Name = g.Name
    }).ToList();

    if (genres == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genres);
});

app.MapGet("/api/patrons", (LoncotesCountyLibDbContext db) => 
{
    var patrons = db.Patrons.Select(p => new PatronDTO
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        Address = p.Address,
        Email = p.Email,
        IsActive = p.IsActive
    }).ToList();

    if (patrons == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(patrons);
});

app.MapGet("/api/patrons/{id}", (LoncotesCountyLibDbContext db, int id) => 
{

    return db.Patrons
    .Where(p => p.Id == id)
    .Select(p => new PatronDTO
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        Address = p.Address,
        Email = p.Email,
        IsActive = p.IsActive,
        Checkouts = db.Checkouts
        .Where(c => c.PatronId == p.Id)        
        .Select(c => new CheckoutDTO
        {
            Id = c.Id,
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate,
            MaterialId = c.MaterialId,
            Material = new MaterialDTO
            {
                Id = c.Material.Id,
                MaterialName = c.Material.MaterialName,
                MaterialTypeId = c.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = c.Material.MaterialType.Id,
                    Name = c.Material.MaterialType.Name,
                    CheckoutDays = c.Material.MaterialType.CheckoutDays
                }
                }
        }).ToList()
    }).SingleOrDefault();

});

app.MapPut("/api/patrons/{id}", (LoncotesCountyLibDbContext db, int id, Patron patron) => 
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(p => p.Id == id);

    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    if (patron.Address != null)
    {
        patronToUpdate.Address = patron.Address;
    }
    if (patron.Email != null)
    {
        patronToUpdate.Email = patron.Email;
    }

    db.SaveChanges();
    return Results.NoContent();

});

app.MapPut("/api/patrons/{id}/deactivate", (LoncotesCountyLibDbContext db, int id) =>
{
    Patron deactivatePatron = db.Patrons.SingleOrDefault(p => p.Id == id);

    if (deactivatePatron.IsActive == true)
    {
        deactivatePatron.IsActive = false;
    }

    db.SaveChanges();
    return Results.NoContent();
});

app.MapPost("/api/checkouts", (LoncotesCountyLibDbContext db, Checkout checkout) =>
{
    var materialDetails = db.Materials.Include(m => m.MaterialType)
    .Where(m => m.Id == checkout.MaterialId)
    .SingleOrDefault();

    if (materialDetails == null)
    {
        return Results.NoContent();
    }

    Checkout newCheckout = new Checkout
    {
        MaterialId = checkout.MaterialId,
        PatronId = checkout.PatronId,
        CheckoutDate = DateTime.Today,
        ReturnDate = DateTime.Today.AddDays(materialDetails.MaterialType.CheckoutDays)

    };
    db.Checkouts.Add(newCheckout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{newCheckout.Id}", newCheckout);    
});

app.MapPut("/api/checkouts/{id}", (LoncotesCountyLibDbContext db, int id) =>
{
    Checkout returnedCheckout = db.Checkouts.SingleOrDefault(c => c.Id == id);

    returnedCheckout.ReturnDate = DateTime.Today;

    db.SaveChanges();
    return Results.NoContent();
});


app.Run();
