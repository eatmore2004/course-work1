using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services;

public class StaffService : GenericService<Staff>, IStaffService
{
    public StaffService(IRepository<Staff> repository) : base(repository)
    {
    }
    
    public async Task<Result<bool>> RegisterStaff(string name, string surname, string email, Position position)
    {
        var allStaff = GetAll().Result;
        
        if (allStaff.Any(s => (s.Name == name && s.Surname == surname) || s.Email == email))
        {
            return new Result<bool>(false, "Staff with this name, surname or email already exists");
        }

        var staff = new Staff(Guid.NewGuid(),position, email, name, surname);

        try
        {
            await Add(staff);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false,e.Message);
        }
    }

    public async Task<Result<bool>> UpdateStaff(string name, string surname, string email, Position position)
    {
        var allStaff = GetAll().Result;
        var staff = allStaff.FirstOrDefault(s => s.Name == name && s.Surname == surname);
        
        if (staff == null)
        {
            return new Result<bool>(false, "Staff with this name and surname does not exist");
        }
        
        staff.StaffPosition = position;
        staff.Email = email;

        try
        {
            await Update(staff.Id,staff);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, e.Message);
        }
    }

    public async Task<Result<bool>> DeleteStaff(string name, string surname)
    {
        var allStaff = GetAll().Result;
        var staff = allStaff.FirstOrDefault(s => s.Name == name && s.Surname == surname);
        
        if (staff == null)
        {
            return new Result<bool>(false, "Staff with this name and surname does not exist");
        }

        try
        {
            await Delete(staff.Id);
            return new Result<bool>(true);
        }
        catch (Exception e)
        {
            return new Result<bool>(false, e.Message);
        }
    }

    public Task<Result<Staff>> GetStaffByName(string name, string surname)
    {
        var allStaff = GetAll().Result;
        var staff = allStaff.FirstOrDefault(s => s.Name == name && s.Surname == surname);
        return Task.FromResult(staff == null ? new Result<Staff>(false) : new Result<Staff>(false, staff));
    }

    public Task<List<Staff>> GetStaffByPosition(Position position)
    {
        var allStaff = GetAll().Result;
        return Task.FromResult(allStaff.Where(s => s.StaffPosition == position).ToList());
    }

    public async Task<Result<string>> BringToPdf()
    {
        var result = await Repository.PackAllToPdf();
        return result;
    }
}