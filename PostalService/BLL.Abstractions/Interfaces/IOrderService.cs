using Core.Models;

namespace BLL.Abstractions.Interfaces;

public interface IOrderService : IGenericService<Order>
{
    Task<Result<bool>> CreateOrder(string orderName, string customerName, string customerSurname);
        
    Task<Result<bool>> AddToOrder(string orderName, string parcelName);
    
    Task<Result<bool>> DeleteOrder(string orderName);
    Task<Result<Order>> GetByName(string orderName);
}
