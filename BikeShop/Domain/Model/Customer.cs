namespace Domain.Model;

public class Customer(string id, string name, string surname, string address, int rentedBikes)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string  Surname { get; set; } = surname;
    public string Address { get; set; } = address;
    public int RentedBikes { get; set; } = rentedBikes;
}