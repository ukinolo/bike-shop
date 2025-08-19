using Domain;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class BikeDbContext(DbContextOptions<DbContext> options) : DbContext(options)
{
    public DbSet<Bike> Bikes { get; set; }
}