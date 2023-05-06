using Core.Enums;

namespace Core.Models;

public class Parcel : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Destination { get; set; }
    public double Weight { get; set; }
    public ParcelType Type { get; set; }
    public Customer Customer { get; set; }

    public Parcel(Guid guid, string name, string description, int destination, double weight, ParcelType type, Customer customer) : base(guid)
    {
        Name = name;
        Description = description;
        Destination = destination;
        Weight = weight;
        Type = type;
        Customer = customer;
    }

    public override string ToString()
    {
        return $"------------------------------\n" +
               $"Name: {Name}\n" +
               $"Description: {Description}\n" +
               $"Destination: {Destination}\n" +
               $"Weight: {Weight}\n" +
               $"Type: {Type}\n" +
               $"Customer: {Customer.Name} {Customer.Surname}\n";
    }
}