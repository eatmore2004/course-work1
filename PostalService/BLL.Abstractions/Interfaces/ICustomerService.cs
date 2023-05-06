using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface ICustomerService : IGenericService<Customer>
    {
        Task<Customer> RegisterCustomer(Customer customer);
        
        Task<Customer> EditCustomer(Customer customer);
        
        Task<List<Customer>> GetCustomersByCardType(Card cardType);
    }
}