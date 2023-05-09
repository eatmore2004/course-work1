using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface ICustomerService : IGenericService<Customer>
    {
        Task<Result<bool>> RegisterCustomer(string name, string surname, string email, Card cardType, double balance);
        
        Task<Result<bool>> UpdateBalance(string name, string surname, double balance);
        
        Task<List<Customer>> GetCustomersByCardType(Card cardType);
        
        Task<List<Customer>> GetCustomersByBalance(double balance);
        
        Task<Result<Customer>> GetCustomerByName(string name, string surname);

        Task<Result<bool>> DeleteCustomer(string name, string surname);
    }
}