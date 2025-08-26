using Domain.Model;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<CustomerDbContext>(options => options.UseNpgsql(connectionString, dbContextBuilder => dbContextBuilder.MigrationsAssembly("CentralShop")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll", policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

{
    using IServiceScope scope = app.Services.CreateScope();

    using CustomerDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    
    dbContext.Database.Migrate();
}

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.MapGet("/customer", (CustomerDbContext db) => db.Customers);

app.MapPost("/customer", async (CustomerCreateDto customerCreate, CustomerDbContext db) =>
{
    if (await db.Customers.AnyAsync(c => c.Id == customerCreate.ID))
        return Results.BadRequest();

    await db.Customers.AddAsync(new Customer(
        customerCreate.ID,
        customerCreate.Name,
        customerCreate.Surname,
        customerCreate.Address,
        0));
    await db.SaveChangesAsync();
    
    return Results.Created();
});

app.MapPut("/customer/{id}/rent", async (string id, CustomerDbContext db) =>
{
    var customer = await db.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
    
    if (customer == null)
    {
        return Results.NotFound("Customer not found");
    }
    if (customer.RentedBikes == 2)
    {
        return Results.BadRequest("You cannot rent");
    }
    
    customer.RentedBikes++;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/customer/{id}/release", async (string id, CustomerDbContext db) =>
{
    var customer = await db.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
    if (customer == null)
    {
        return Results.BadRequest("Customer not found");
    }
    customer.RentedBikes = customer.RentedBikes == 0 ? 0 : customer.RentedBikes - 1;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/ping", () => Results.Ok());

app.Run();

internal record CustomerCreateDto(string ID, string Name, string Surname, string Address);