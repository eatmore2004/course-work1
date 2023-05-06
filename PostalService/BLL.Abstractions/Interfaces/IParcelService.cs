using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces;

public interface IParcelService : IGenericService<Parcel>
{
    Task<Result<bool>> CreateParcel(string name, string description, int destination, double weight, ParcelType type, string customerName, string customerSurname);
    
    Task<List<Parcel>> GetParcelsByCustomer(string customerName);
    
    Task<List<Parcel>> GetParcelsByName(string name);
    
    Task<List<Parcel>> GetParcelsByType(ParcelType  parcelType);
    
    Task<List<Parcel>> GetParcelsByDestination(string destination);
    
    Task<List<Parcel>> GetParcelsByWeight(double weight);
    
    Task<Result<bool>> DeleteParcel(string parcelName);
}