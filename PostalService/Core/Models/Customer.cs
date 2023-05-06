using Core.Enums;

namespace Core.Models;

public class Customer : BaseEntity
{
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Card SubscriptionType { get; set; }

    public Customer(Guid guid, string passwordHash, string name, string surname, Card subscription) : base(guid)
    {
        PasswordHash = passwordHash;
        Name = name;
        Surname = surname;
        SubscriptionType = subscription;
    }

    public override string ToString()
    {
        return "------------------------------\n" +
               $"Name: {Name}\n" +
               $"Surname: {Surname}\n" +
               $"Subscription: {SubscriptionType}\n";
    }
}