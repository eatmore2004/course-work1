using BLL.Abstractions.Interfaces;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class OrderConsoleManager : ConsoleManager<IOrderService, Order>, IConsoleManager<Order>
    {
        public OrderConsoleManager(IOrderService orderService) : base(orderService)
        {
        }

        public override async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
            {
                { "1", DisplayAllUsersAsync },
                { "2", CreateUserAsync },
                { "3", UpdateUserAsync },
                { "4", DeleteUserAsync },
            };

            while (true)
            {
                Console.Clear();
                ConsoleHandler.PrintCaption("Orders management");
                ConsoleHandler.PrintInfo(" => 1. Display all users");
                ConsoleHandler.PrintInfo(" => 2. Create a new user");
                ConsoleHandler.PrintInfo(" => 3. Update a user");
                ConsoleHandler.PrintInfo(" => 4. Delete a user");
                ConsoleHandler.PrintInfo(" => 5. Exit\n");

                ConsoleHandler.Print("Enter the operation number: ");
                var input = Console.ReadLine();

                if (input == "5")
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

        private async Task DisplayAllUsersAsync()
        {

        }

        private async Task CreateUserAsync()
        {
            
        }

        private async Task UpdateUserAsync()
        {
        }

        private async Task DeleteUserAsync()
        {
        }

    }
}