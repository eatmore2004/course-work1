using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services;

public class OrderService : GenericService<Order>, IOrderService
{
    public OrderService(IRepository<Order> repository) : base(repository)
    {
    }

    public Task<Order> CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<Order> EditOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<Order> AddToOrder(Parcel parcel)
    {
        throw new NotImplementedException();
    }
}