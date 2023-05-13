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

    public async Task<Result<bool>> CreateParcel(string name, string description, int destination, double weight, ParcelType type)
    {
        var allParcels = GetAll().Result;
        
        if (allParcels.Any(p => p.Name == name))
        {
            return new Result<bool>(false, "Parcel with this name exists");
        }

        var parcel = new Parcel(Guid.NewGuid(),name,description,destination,weight,type);

        try
        {
            await Add(parcel);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public Task<List<Parcel>> GetParcelsByType(ParcelType parcelType)
    {
        var allParcels = GetAll().Result;
        return Task.FromResult(allParcels.Where(p => p.Type == parcelType).ToList());
    }

    public Task<List<Parcel>> GetParcelsByDestination(int destination)
    {
        var allParcels = GetAll().Result;
        return Task.FromResult(allParcels.Where(p => p.Destination == destination).ToList());
    }

    public Task<List<Parcel>> GetParcelsByWeight(double weight)
    {
        var allParcels = GetAll().Result;
        return Task.FromResult(allParcels.Where(p => p.Weight <= weight).ToList());
    }

    public async Task<Result<Parcel>> GetParcelByName(string name)
    {
        var allParcels = GetAll().Result;
        return (allParcels.Any(p => p.Name == name)) ? 
            new Result<Parcel>(true, allParcels.First(p => p.Name == name)) : 
            new Result<Parcel>(false,"Parcel not found");
    }

    public async Task<Result<bool>> DeleteParcel(string parcelName)
    {
        var allParcel = GetAll().Result;
        var parcel = allParcel.FirstOrDefault(p => p.Name == parcelName);
        
        if (parcel == null)
        {
            return new Result<bool>(false, "Parcel with this name does not exist");
        }

        try
        {
            await Delete(parcel.Id);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, e.Message);
        }
    }

    public async Task<Result<string>> BringToPdf()
    {
        var result = await Repository.PackAllToPdf();
        return result;
    }
}