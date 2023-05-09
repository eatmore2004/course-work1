using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces;

public interface IParcelService : IGenericService<Parcel>
{
    Task<Result<bool>> CreateParcel(string name, string description, int destination, double weight, ParcelType type);

    Task<List<Parcel>> GetParcelsByType(ParcelType  parcelType);
    
    Task<List<Parcel>> GetParcelsByDestination(int destination);
    
    Task<List<Parcel>> GetParcelsByWeight(double weight);
    
    Task<Result<Parcel>> GetParcelByName(string name);
    
    Task<Result<bool>> DeleteParcel(string parcelName);
}