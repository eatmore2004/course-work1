using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services;

public class ParcelService : GenericService<Parcel>, IParcelService
{
    public ParcelService(IRepository<Parcel> repository) : base(repository)
    {
    }

    public Task<Result<bool>> CreateParcel(string name, string description, int destination, double weight, ParcelType type, string customerName,
        string customerSurname)
    {
        throw new NotImplementedException();
    }

    public Task<List<Parcel>> GetParcelsByCustomer(string customerName)
    {
        throw new NotImplementedException();
    }

    public Task<List<Parcel>> GetParcelsByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<List<Parcel>> GetParcelsByType(ParcelType parcelType)
    {
        throw new NotImplementedException();
    }

    public Task<List<Parcel>> GetParcelsByDestination(string destination)
    {
        throw new NotImplementedException();
    }

    public Task<List<Parcel>> GetParcelsByWeight(double weight)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteParcel(string parcelName)
    {
        throw new NotImplementedException();
    }
}