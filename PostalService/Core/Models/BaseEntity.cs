namespace Core.Models;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    protected BaseEntity(Guid guid)
    {
        Id = guid;
    }
}