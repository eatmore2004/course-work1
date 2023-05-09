using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using UI.ConsoleHandlers;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class CustomerConsoleManager : ConsoleManager<ICustomerService, Customer>, IConsoleManager<Customer>
    {
        public CustomerConsoleManager(ICustomerService userService) : base(userService)
        {
        }

        public override async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
            {
                { "1", DisplayAllCustomersAsync },
                { "2", DisplayAllByCardTypeAsync },
                { "3", DisplayAllByBalanceAsync },
                { "4", CreateCustomerAsync },
                { "5", UpdateCustomerAsync },
                { "6", DeleteCustomerAsync },
            };

            while (true)
            {
                ConsoleHandler.Clear();
                ConsoleHandler.PrintCaption(@"
                                 ██████╗██╗   ██╗███████╗████████╗ ██████╗ ███╗   ███╗███████╗██████╗ ███████╗
                                ██╔════╝██║   ██║██╔════╝╚══██╔══╝██╔═══██╗████╗ ████║██╔════╝██╔══██╗██╔════╝
                                ██║     ██║   ██║███████╗   ██║   ██║   ██║██╔████╔██║█████╗  ██████╔╝███████╗
                                ██║     ██║   ██║╚════██║   ██║   ██║   ██║██║╚██╔╝██║██╔══╝  ██╔══██╗╚════██║
                                ╚██████╗╚██████╔╝███████║   ██║   ╚██████╔╝██║ ╚═╝ ██║███████╗██║  ██║███████║
                                 ╚═════╝ ╚═════╝ ╚══════╝   ╚═╝    ╚═════╝ ╚═╝     ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝
                                          ");
                ConsoleHandler.PrintInfo(" |=> 1. Display all customers");
                ConsoleHandler.PrintInfo(" |=> 2. Display all customers by card type");
                ConsoleHandler.PrintInfo(" |=> 3. Display all customers by balance");
                ConsoleHandler.PrintInfo(" |=> 4. Create a new customer");
                ConsoleHandler.PrintInfo(" |=> 5. Update a customer balance");
                ConsoleHandler.PrintInfo(" |=> 6. Delete a customer");
                ConsoleHandler.PrintInfo(" <= 9. Back to Main Menu\n");

                ConsoleHandler.Print("Enter the operation number: ");
                var input = Console.ReadLine();
                ConsoleHandler.Clear();

                if (input == "9")
                {
                    break;
                }

                if (actions.ContainsKey(input))
                {
                    await actions[input]();
                    ConsoleHandler.Print("Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid operation number.");
                }
            }
        }

        private async Task DisplayAllByBalanceAsync()
        {
            var balance = ConsoleHandler.AskForDouble("Enter boundary (min) balance:");
            ConsoleHandler.Clear();
            var customers = Service.GetCustomersByBalance(balance).Result;
            ConsoleHandler.PrintCollection(customers);
        }

        private async Task DisplayAllByCardTypeAsync()
        {
            var cardType = ConsoleHandler.AskForEnum<Card>("Enter card type:");
            ConsoleHandler.Clear();
            var customers = Service.GetCustomersByCardType(cardType).Result;
            ConsoleHandler.PrintCollection(customers);
        }

        private Task DeleteCustomerAsync()
        {
            var name = ConsoleHandler.AskForString("Enter name:",@"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter surname:",@"^[a-zA-Z]{4,}$");
            
            ConsoleHandler.Clear();
            
            var result = Service.DeleteCustomer(name, surname).Result;

            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Customer deleted successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }

            return Task.CompletedTask;
        }

        private async Task UpdateCustomerAsync()
        {
            var name = ConsoleHandler.AskForString("Enter name:",@"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter surname:",@"^[a-zA-Z]{4,}$");
            var balance = ConsoleHandler.AskForDouble("Update balance:");
            
            ConsoleHandler.Clear();
            
            var result = await Service.UpdateBalance(name, surname, balance);

            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Customer balance updated successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task CreateCustomerAsync()
        {
            var name = ConsoleHandler.AskForString("Enter name:",@"^[a-zA-Z]{4,}$");
            var surname = ConsoleHandler.AskForString("Enter surname:",@"^[a-zA-Z]{4,}$");
            var email = ConsoleHandler.AskForString("Enter the email", @"^[a-zA-Z0-9]{4,}@[a-zA-Z]{4,}\.[a-zA-Z]{2,}$");
            var balance = ConsoleHandler.AskForDouble("Enter balance:");
            var cardType = ConsoleHandler.AskForEnum<Card>("Enter customer card type:");
            
            ConsoleHandler.Clear();
            
            var result = await Service.RegisterCustomer(name, surname, email, cardType, balance);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Customer created successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private Task DisplayAllCustomersAsync()
        {
            var customers = Service.GetAll().Result;
            ConsoleHandler.PrintCollection(customers);
            return Task.CompletedTask;
        }
    }
}