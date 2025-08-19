using Domain;
using Domain.Model;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BikeDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//TODO inject the http client to send requests to check for CentralShop

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/bike", (BikeDbContext db) => db.Bikes);

app.MapPost("/bike", async (BikeCreateDto bikeCreateDto, BikeDbContext db) =>
{
    //TODO
    //Add check for Customer existence
    //If NotFound, send create and rent, and then same
    //If Bad Request, return Bad Request
    //If Ok, just save new Bike
    
    var bike = new Bike(
        bikeCreateDto.Id,
        bikeCreateDto.Type,
        bikeCreateDto.Rented,
        bikeCreateDto.CustomerId);
    await db.Bikes.AddAsync(bike);
    await db.SaveChangesAsync();
});

app.MapPut("/bike/{id}", async (string id, BikeDbContext db) =>
{
    var bike = await db.Bikes.Where(bike => bike.Id == id).FirstOrDefaultAsync();
    if (bike == null)
    {
        return Results.NotFound();
    }

    if (bike.Returned)
    {
        return Results.NoContent();
    }

    //TODO
    //Send request to return bike
    bike.Returned = true;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

internal record BikeCreateDto(string Id, string Type, DateTime Rented, string CustomerId);