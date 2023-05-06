using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services;

public class CustomerService : GenericService<Customer>, ICustomerService
{
    public CustomerService(IRepository<Customer> repository) : base(repository)
    {
    }

    public Task<Customer> RegisterCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> EditCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task<List<Customer>> GetCustomersByCardType(Card cardType)
    {
        throw new NotImplementedException();
    }
}