using Core.Enums;

namespace Core.Models;

public class Staff : BaseEntity
{
    public Position StaffPosition { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public Staff(Guid guid, Position staffPosition, string email, string name, string surname) : base(guid)
    {
        StaffPosition = staffPosition;
        Email = email;
        Name = name;
        Surname = surname;
    }

    public override string ToString()
    {
        return $" Staff position: {StaffPosition}\n" +
               $" Name: {Name} {Surname}\n" +
               $" Email: {Email}\n";
    }   
}