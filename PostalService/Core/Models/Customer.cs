using Core.Enums;

namespace Core.Models;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public Card SubscriptionType { get; set; }
    public double Balance { get; set; }

    public Customer(Guid guid, string name, string surname, Card subscription, double balance, string email) : base(guid)
    {
        Name = name;
        Surname = surname;
        SubscriptionType = subscription;
        Balance = balance;
        Email = email;
    }

    public override string ToString()
    {
        return $" Name: {Name} {Surname}\n" +
               $" Email: {Email}\n" +
               $" Subscription: {SubscriptionType}\n" +
               $" Balance (EUR): {Balance}\n";
    }
}