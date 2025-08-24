using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}