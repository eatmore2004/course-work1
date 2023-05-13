using Core.Enums;

namespace Core.Models;

public class Parcel : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Destination { get; set; }
    public double Weight { get; set; }
    public ParcelType Type { get; set; }
    public bool IsLocked { get; set; }

    public Parcel(Guid guid, string name, string description, int destination, double weight, ParcelType type) : base(guid)
    {
        Name = name;
        Description = description;
        Destination = destination;
        Weight = weight;
        Type = type;
        IsLocked = false;
    }
    
    public double Price()
    {
        var multiplier = Type switch
        {
            ParcelType.Standard => 1,
            ParcelType.Fast => 1.2,
            ParcelType.Express => 1.5,
            ParcelType.Priority => 2.0,
            _ => throw new ArgumentOutOfRangeException()
        };

        var price = Math.Round(Weight * multiplier, 2, MidpointRounding.AwayFromZero);
        return (price < 5) ? 5 * multiplier : price;
    }

    public override string ToString()
    {
        return $" Name: {Name}\n" +
               $" Description: {Description}\n" +
               $" Destination: {Destination}\n" +
               $" Weight: {Weight}\n" +
               $" Type: {Type}\n" +
               $" Price: {Price()} EUR\n" +
               $" In order: {IsLocked}\n";
    }
}