using Domain;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class CustomerDbContext(DbContextOptions<DbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}