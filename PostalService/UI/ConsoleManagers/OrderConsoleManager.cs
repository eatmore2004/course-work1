using System.Diagnostics;
using BLL.Abstractions.Interfaces;
using Core.Models;
using UI.ConsoleHandlers;
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
            var actions = new Dictionary<string, Func<Task>>
            {
                { "1", DisplayAllOrdersAsync },
                { "2", DisplayByName },
                { "3", CreateOrderAsync },
                { "4", UpdateOrderAsync },
                { "5", BringToPdfAsync },
                { "6", DeleteOrderAsync },
            };

            while (true)
            {
                ConsoleHandler.Clear();
                ConsoleHandler.PrintCaption(@"
                                ██████╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗
                                ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝
                                ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝███████╗
                                ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗╚════██║
                                ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║███████║
                                 ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝                              
                                ");
                ConsoleHandler.PrintInfo(" |=> 1. Display all orders");
                ConsoleHandler.PrintInfo(" |=> 2. Display order by name");
                ConsoleHandler.PrintInfo(" |=> 3. Create a new order");
                ConsoleHandler.PrintInfo(" |=> 4. Add a parcel to order");
                ConsoleHandler.PrintInfo(" |=> 5. Bring all to PDF");
                ConsoleHandler.PrintInfo(" |=> 6. Delete order");
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

        private async Task BringToPdfAsync()
        {
            var result = await Service.BringToPdf();
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess($"Successfully packed to PDF ({result.Data})");
                Process.Start(new ProcessStartInfo
                {
                    FileName = result.Data,
                    UseShellExecute = true
                });
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task DisplayByName()
        {
            var orderName = ConsoleHandler.AskForString("Enter the order name", @"^[a-zA-Z]{4,}$");
            var orderResult = await Service.GetByName(orderName);
            
            ConsoleHandler.Clear();
            
            if (orderResult.IsSuccessful)
            {
                ConsoleHandler.Print(orderResult.Data.ToString());
            }
            else
            {
                ConsoleHandler.RaiseError(orderResult.Message);
            }
        }

        private async Task DisplayAllOrdersAsync()
        {
            var orders = await Service.GetAll();
            ConsoleHandler.PrintCollection(orders);
        }

        private async Task CreateOrderAsync()
        {
            var orderName = ConsoleHandler.AskForString("Enter the order name", @"^[a-zA-Z]{4,}$");
            var customerName = ConsoleHandler.AskForString("Enter the customer name", @"^[a-zA-Z]{4,}$");
            var customerSurname = ConsoleHandler.AskForString("Enter the customer surname", @"^[a-zA-Z]{4,}$");
            
            ConsoleHandler.Clear();
            
            var result = await Service.CreateOrder(orderName, customerName, customerSurname);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Order created successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task UpdateOrderAsync()
        {
            var orderName = ConsoleHandler.AskForString("Enter the order name", @"^[a-zA-Z]{4,}$");
            var parcelName = ConsoleHandler.AskForString("Enter the parcel name", @"^[a-zA-Z]{4,}$");
            
            ConsoleHandler.Clear();
            
            var result = await Service.AddToOrder(orderName, parcelName);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess($"{parcelName} added to {orderName} successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
            
        }

        private async Task DeleteOrderAsync()
        {
            var orderName = ConsoleHandler.AskForString("Enter the order name", @"^[a-zA-Z]{4,}$");
            
            ConsoleHandler.Clear();
            
            var result = await Service.DeleteOrder(orderName);

            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Order deleted successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

    }
}