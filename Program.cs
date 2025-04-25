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

app.MapGet("/api/materials", (int? materialTypeId, int? genreId, LoncotesCountyLibDbContext db) => 
{
    // ! Need to logic through using query params established above. 
    // ! Could do if statements? Commented out code at bottom works but I didn't write it...
    return Results.Ok(db.Materials
    .Include(m => m.MaterialType)
    .Include(m => m.Genre)
    .Where(g => g.OutOfCirculationSince == null)
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
    }).ToList());

    //     IQueryable<Material> query = db.Materials
    //     .Include(m => m.MaterialType)
    //     .Include(m => m.Genre)
    //     .Where(m => m.OutOfCirculationSince == null);
    
    // // Apply filters only if parameters are provided
    // if (materialTypeId != null)
    // {
    //     query = query.Where(m => m.MaterialTypeId == materialTypeId);
    // }
    
    // if (genreId != null)
    // {
    //     query = query.Where(m => m.GenreId == genreId);
    // }
    
    // List<MaterialDTO> materials = query.Select(m => new MaterialDTO
    // {
    //     Id = m.Id,
    //     MaterialName = m.MaterialName,
    //     MaterialTypeId = m.MaterialTypeId,
    //     MaterialType = new MaterialTypeDTO
    //     {
    //         Id = m.MaterialType.Id,
    //         Name = m.MaterialType.Name,
    //         CheckoutDays = m.MaterialType.CheckoutDays
    //     },
    //     GenreId = m.GenreId,
    //     Genre = new GenreDTO
    //     {
    //         Id = m.Genre.Id,
    //         Name = m.Genre.Name
    //     },
    //     OutOfCirculationSince = m.OutOfCirculationSince
    // }).ToList();
    
    // return Results.Ok(materials);
});



app.MapPost("/api/materials", (LoncotesCountyLibDbContext db, Material material) =>
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

app.Run();

// ! Current status, Working on step 1 of chpt 2
// ? Just realized that I seeded the db with materials that ALL have out of circulation dates...

// ? https://github.com/nashville-software-school/server-side-dotnet-curriculum/blob/main/book-3-sql-efcore/chapters/loncotes-basic-features.md