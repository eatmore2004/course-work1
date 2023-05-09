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

    public async Task<Result<bool>> RegisterCustomer(string name, string surname, string email, Card cardType, double balance)
    {
        var allCustomers = GetAll().Result;
        
        if (allCustomers.Any(s => (s.Name == name && s.Surname == surname) || s.Email == email))
        {
            return new Result<bool>(false, "Customer with this name, surname or email already exists");
        }

        var customer = new Customer(Guid.NewGuid(), name, surname, cardType, balance, email);

        try
        {
            await Add(customer);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public async Task<Result<bool>> UpdateBalance(string name, string surname, double balance)
    {
        var allCustomers = GetAll().Result;
        var customer = allCustomers.FirstOrDefault(s => s.Name == name && s.Surname == surname);
        
        if (customer == null)
        {
            return new Result<bool>(false, "Customer with this name and surname does not exist");
        }
        
        customer.Balance = balance;

        try
        {
            await Update(customer.Id,customer);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, e.Message);
        }
    }

    public Task<List<Customer>> GetCustomersByCardType(Card cardType)
    {
        var allCustomers = GetAll().Result;
        return Task.FromResult(allCustomers.Where(c => c.SubscriptionType == cardType).ToList());
    }

    public Task<List<Customer>> GetCustomersByBalance(double balance)
    {
        var allCustomers = GetAll().Result;
        return Task.FromResult(allCustomers.Where(c => c.Balance >= balance).ToList());
    }

    public async Task<Result<Customer>> GetCustomerByName(string name, string surname)
    {
        var allCustomers = GetAll().Result;
        return allCustomers.Any(c => c.Name == name && c.Surname == surname) ? 
            new Result<Customer>(true, allCustomers.First(c => c.Name == name && c.Surname == surname)) : 
            new Result<Customer>(false,"Customer not found");
    }

    public async Task<Result<bool>> DeleteCustomer(string name, string surname)
    {
        var allCustomers = GetAll().Result;
        var customer = allCustomers.FirstOrDefault(s => s.Name == name && s.Surname == surname);
        
        if (customer == null)
        {
            return new Result<bool>(false, "Customer with this name and surname does not exist");
        }

        try
        {
            await Delete(customer.Id);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, e.Message);
        }
    }
}