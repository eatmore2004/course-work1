namespace Core.Models;

public class Order : BaseEntity
{
    public string Name { get; set; }
    public int Tariff { get; set; }
    public ICollection<Parcel> Parcels { get; set; }
    
    public Order(Guid guid, string name, int tariff, ICollection<Parcel> parcels) : base(guid)
    {
        Name = name;
        Tariff = tariff;
        Parcels = parcels;
    }

    public double GetPrice()
    {
        double totalPrice = 0;
        foreach (var parcel in Parcels) 
        {
            totalPrice += parcel.Weight * Tariff;
        }

        return totalPrice;
    }

    public override string ToString()
    {
        return "------------------------------\n" +
               $"Name: {Name}\n" +
               $"Tariff: {Tariff}\n" +
               $"Parcels: {Parcels}\n";
    }
}