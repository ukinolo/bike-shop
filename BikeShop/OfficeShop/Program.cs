using Domain.Model;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string[] cities = ["NoviSad", "Kragujevac", "Subotica"];

string? city = null;

if (args.Length >= 1 && cities.Contains(args[0]))
{
    city = args[0];
}
else
{
    city = cities[0];
}

var connectionString = builder.Configuration.GetConnectionString(city);

// Add services to the container.
builder.Services.AddDbContext<BikeDbContext>(options => options.UseNpgsql(connectionString, dbContextBuilder => dbContextBuilder.MigrationsAssembly("OfficeShop")));

//TODO inject the http client to send requests to check for CentralShop

var app = builder.Build();

{
    using IServiceScope scope = app.Services.CreateScope();

    using BikeDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<BikeDbContext>();
    
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.MapGet("/bike", (BikeDbContext db) => db.Bikes);

app.MapPost("/bike", async (BikeCreateDto bikeCreateDto, BikeDbContext db) =>
{
    //TODO
    //Add check for Customer existence
    //If NotFound, return Bad Request
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

app.MapPut("/bike/{id}/return", async (string id, BikeDbContext db) =>
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

app.MapGet("/ping", () => Results.Ok(connectionString));

app.Run();

internal record BikeCreateDto(string Id, string Type, DateTime Rented, string CustomerId);