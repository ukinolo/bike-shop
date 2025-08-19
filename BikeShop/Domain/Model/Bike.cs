namespace Domain.Model;

public class Bike(string id, string type, DateTime rented, string customerId)
{
    public string Id { get; set; } = id;
    public string Type { get; set; } = type;
    public DateTime Rented { get; set; } = rented;
    public string CustomerId { get; set; } = customerId;
    public bool Returned { get; set; } = false;
}