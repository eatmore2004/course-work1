using BLL.Abstractions.Interfaces;
using BLL.Services;
using Core.Enums;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class StaffConsoleManager : ConsoleManager<IStaffService, Staff>, IConsoleManager<Staff>
    {
        public StaffConsoleManager(IStaffService staffService) : base(staffService)
        {
        }

        public override async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
            {
                { "1", DisplayAllStaffAsync },
                { "2", DisplayByRoleAsync },
                { "3", CreateStaffAsync },
                { "4", UpdateStaffAsync },
                { "5", DeleteStaffAsync },
            };
            
            Console.Clear();
            
            while (true)
            {
                ConsoleHandler.PrintCaption("\nStaff management");
                ConsoleHandler.PrintInfo(" => 1. Display all staff");
                ConsoleHandler.PrintInfo(" => 2. Display by role staff");
                ConsoleHandler.PrintInfo(" => 3. Create a new staff");
                ConsoleHandler.PrintInfo(" => 4. Update a staff");
                ConsoleHandler.PrintInfo(" => 5. Delete a staff");
                ConsoleHandler.PrintInfo(" => 6. Exit\n");

                ConsoleHandler.Print("Enter the operation number: ");
                var input = Console.ReadLine();
                Console.Clear();
                
                if (input == "6")
                {
                    break;
                }

                if (actions.ContainsKey(input))
                {
                    await actions[input]();
                }
                else
                {
                    Console.WriteLine("Invalid operation number.");
                }
            }
        }

        private async Task DisplayByRoleAsync()
        {
            var position = ConsoleHandler.AskForEnum<Position>("Provide the position");
            var staff = await Service.GetStaffByPosition(position);
            if (staff.Count != 0)
            {
                ConsoleHandler.RaiseSuccess("\nAll users:");  
                ConsoleHandler.PrintCollection(staff);
            }
            else
            {
                ConsoleHandler.RaiseError("No staff found.");
            }
        }

        private async Task DisplayAllStaffAsync()
        {
              
            var staff = await Service.GetAll();
            if (staff.Count != 0)
            {
                ConsoleHandler.RaiseSuccess("\nAll users:");  
                ConsoleHandler.PrintCollection(staff);
            }
            else
            {
                ConsoleHandler.RaiseError("No staff found.");
            }
        }

        private async Task CreateStaffAsync()
        {
            var position = ConsoleHandler.AskForEnum<Position>("Enter the position");
            var name = ConsoleHandler.AskForString("Enter the name", @"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter the surname", @"^[a-zA-Z]{4,}$");
            var email = ConsoleHandler.AskForString("Enter the email", @"^[a-zA-Z0-9]{4,}@[a-zA-Z]{4,}\.[a-zA-Z]{2,}$");
            
            Console.Clear();
            
            var result = await Service.RegisterStaff(name, surname, email, position);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Staff created successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task UpdateStaffAsync()
        {
            var name = ConsoleHandler.AskForString("Enter the name", @"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter the surname", @"^[a-zA-Z]{4,}$");
            var email = ConsoleHandler.AskForString("Update the email", @"^[a-zA-Z0-9]{4,}@[a-zA-Z]{4,}\.[a-zA-Z]{2,}$");
            var position = ConsoleHandler.AskForEnum<Position>("Update the position");
            
            Console.Clear();
            
            var result = await Service.UpdateStaff(name, surname, email, position);

            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Staff updated successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task DeleteStaffAsync()
        {
            var name = ConsoleHandler.AskForString("Enter the name", @"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter the surname", @"^[a-zA-Z]{4,}$");
            
            Console.Clear();
            
            var staffResult = Service.DeleteStaff(name, surname);
            
            if (staffResult.Result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Staff deleted successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError($"No {name} {surname} found.");
            }
        }

    }
}