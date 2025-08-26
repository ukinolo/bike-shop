using Domain.Model;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using OfficeShop;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll", policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddHttpClient<CentralShopHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("CentralShopApi"));
});

var app = builder.Build();

{
    using IServiceScope scope = app.Services.CreateScope();

    using BikeDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<BikeDbContext>();
    
    dbContext.Database.Migrate();
}

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.MapGet("/bike", (BikeDbContext db) => db.Bikes);

app.MapPost("/bike", async (BikeCreateDto bikeCreateDto, BikeDbContext db, CentralShopHttpClient client) =>
{
    var (success, error) = await client.RetBike(bikeCreateDto.CustomerId);
    if (!success)
    {
        return Results.BadRequest(error);
    }
    
    var bike = new Bike(
        bikeCreateDto.Id,
        bikeCreateDto.Type,
        bikeCreateDto.Rented,
        bikeCreateDto.CustomerId);
    await db.Bikes.AddAsync(bike);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/bike/{id}/return", async (string id, BikeDbContext db, CentralShopHttpClient client) =>
{
    var bike = await db.Bikes.Where(bike => bike.Id == id).FirstOrDefaultAsync();
    if (bike == null)
    {
        return Results.NotFound("Bike not found");
    }

    if (bike.Returned)
    {
        return Results.NoContent();
    }

    var (success, error) = await client.ReleaseBike(bike.CustomerId);
    if (!success)
    {
        return Results.BadRequest(error);
    }
    bike.Returned = true;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/ping", () => Results.Ok(connectionString));

app.Run();

internal record BikeCreateDto(string Id, string Type, DateTime Rented, string CustomerId);