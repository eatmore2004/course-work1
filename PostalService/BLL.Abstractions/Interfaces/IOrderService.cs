using Core.Models;

namespace BLL.Abstractions.Interfaces;

public interface IOrderService : IGenericService<Order>
{
    Task<Order> CreateOrder(Order order);
    
    Task<Order> EditOrder(Order order);
    
    Task<Order> AddToOrder(Parcel parcel);
}