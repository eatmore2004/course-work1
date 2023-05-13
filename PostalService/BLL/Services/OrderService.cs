using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services;

public class OrderService : GenericService<Order>, IOrderService
{
    private readonly ICustomerService _сustomerService;
    private readonly IParcelService _parcelService;
    
    public OrderService(IRepository<Order> repository, ICustomerService сustomerService, IParcelService parcelService) : base(repository)
    {
        _сustomerService = сustomerService;
        _parcelService = parcelService;
    }

    public async Task<Result<bool>> CreateOrder(string orderName, string customerName, string customerSurname)
    {
        var allOrders = GetAll().Result;
        
        if (allOrders.Any(p => p.Name == orderName))
        {
            return new Result<bool>(false, "Order with this name exists");
        }

        var customerResult = _сustomerService.GetCustomerByName(customerName, customerSurname).Result;

        if (!customerResult.IsSuccessful) return new Result<bool>(false, "Customer not found");
        
        var order = new Order(Guid.NewGuid(),orderName,customerResult.Data);
        try
        {
            await Add(order);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public async Task<Result<bool>> AddToOrder(string orderName, string parcelName)
    {
        var allOrders = GetAll().Result;
        var order = allOrders.FirstOrDefault(o => o.Name == orderName);
        
        if (order == null)
        {
            return new Result<bool>(false, "Order with this name does not exist");
        }

        var parcelResult = _parcelService.GetParcelByName(parcelName).Result;

        if (!parcelResult.IsSuccessful) return new Result<bool>(false, "Parcel not found");
        
        var parcel = parcelResult.Data;
        if (parcel.IsLocked) return new Result<bool>(false, "Parcel is already locked");

        order.Parcels.Add(parcelResult.Data);
        parcel.IsLocked = true;
        
        try
        {
            await Update(order.Id,order);
            await _parcelService.Update(parcel.Id,parcel);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public async Task<Result<bool>> DeleteOrder(string orderName)
    {
        var allOrders = GetAll().Result;
        var order = allOrders.FirstOrDefault(o => o.Name == orderName);
        
        if (order == null)
        {
            return new Result<bool>(false, "Order with this name does not exist");
        }

        try
        {
            foreach (var parcel in order.Parcels)
            {
                parcel.IsLocked = false;
                await _parcelService.Update(parcel.Id,parcel);
            }
            
            await Delete(order.Id);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public async Task<Result<Order>> GetByName(string orderName)
    {
        var allOrders = GetAll().Result;
        return (allOrders.Any(p => p.Name == orderName)) ? 
            new Result<Order>(true, allOrders.First(p => p.Name == orderName)) : 
            new Result<Order>(false,"Order not found");
    }

    public async Task<Result<string>> BringToPdf()
    {
        var result = await Repository.PackAllToPdf();
        return result;
    }
}