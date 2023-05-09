using System.Text;
using Core.Enums;

namespace Core.Models;

public class Order : BaseEntity
{
    public string Name { get; set; }
    public Customer Customer { get; set; }
    public ICollection<Parcel> Parcels { get; set; }
    
    public Order(Guid guid, string name, Customer customer) : base(guid)
    {
        Name = name;
        Parcels = new List<Parcel>();
        Customer = customer;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        var multiplier = Customer.SubscriptionType switch
        {
            Card.Standard => 0,
            Card.Gold => 5,
            Card.Platinum => 10,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        sb.AppendLine("------------------------------");
        sb.AppendLine($"| Order name: {Name} ");
        sb.AppendLine($"| Customer: {Customer.Name} {Customer.Surname} ");
        sb.AppendLine($"| Customer card discount: {multiplier}% ");

        if (Parcels.Count == 0) return sb.ToString();

        sb.AppendLine("| All parcels: ");

        var totalPrice = 0.0;

        foreach (var parcel in Parcels)
        {
            var price = parcel.Price();
            totalPrice += price;
            sb.AppendLine($"|   + {parcel.Name} ({parcel.Weight} kg) = {price} EUR");
        }

        var cardDiscount = Math.Round(totalPrice * multiplier / 100, 2, MidpointRounding.AwayFromZero);
        
        var finalPrice = totalPrice - cardDiscount;
        
        sb.AppendLine($"| Total price: {totalPrice} EUR");
        sb.AppendLine($"|   - Card discount {cardDiscount} EUR");
        sb.AppendLine($"| Final price: {finalPrice} EUR");
        sb.AppendLine("------------------------------");
        
        return sb.ToString();
    }
}