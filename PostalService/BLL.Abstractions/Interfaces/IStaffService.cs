using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces;

public interface IStaffService : IGenericService<Staff>
{
    Task<Result<bool>> RegisterStaff(string name, string surname, string email, Position position);
    
    Task<Result<bool>> UpdateStaff(string name, string surname, string email, Position position);
    
    Task<Result<bool>> DeleteStaff(string name, string surname);
         
    Task<Result<Staff>> GetStaffByName(string name, string surname);
 
    Task<List<Staff>> GetStaffByPosition(Position position);
}