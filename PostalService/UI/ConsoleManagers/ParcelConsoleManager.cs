using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class ParcelConsoleManager : ConsoleManager<IParcelService, Parcel>, IConsoleManager<Parcel>
    {
        public ParcelConsoleManager(IParcelService parcelService) : base(parcelService)
        {
        }

        public override async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
            {
                { "1", DisplayAllParcelsAsync },
                { "2", DisplayAllByTypeAsync },
                { "3", DisplayAllByCustomerAsync },
                { "4", DisplayAllByWeightAsync },
                { "5", DisplayAllByDestinationAsync },
                { "6", CreateParcelAsync },
                { "7", DeleteParcelAsync },
            };

            Console.Clear();
            
            while (true)
            {
                ConsoleHandler.PrintCaption("Parcels management");
                ConsoleHandler.PrintInfo(" |=> 1. Display all parcels");
                ConsoleHandler.PrintInfo(" |=> 2. Display all parcels by type");
                ConsoleHandler.PrintInfo(" |=> 3. Display all parcels by customer");
                ConsoleHandler.PrintInfo(" |=> 4. Display all parcels by weight");
                ConsoleHandler.PrintInfo(" |=> 5. Display all parcels by destination");
                ConsoleHandler.PrintInfo(" |=> 6. Create a new user");
                ConsoleHandler.PrintInfo(" |=> 7. Delete a user");
                ConsoleHandler.PrintInfo(" |=> 8. Exit\n");;

                ConsoleHandler.Print("Enter the operation number: ");
                var input = Console.ReadLine();
                Console.Clear();

                if (input == "8")
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

        private async Task DisplayAllParcelsAsync()
        {
            var parcels = await Service.GetAll();
            ConsoleHandler.PrintCollection(parcels);
        }
        
        private async Task DisplayAllByDestinationAsync()
        {
            var destination = ConsoleHandler.AskForString("Enter destination:",@"^[a-zA-Z]{4,}$");
            var parcels = await Service.GetParcelsByDestination(destination);
            ConsoleHandler.PrintCollection(parcels);
        }

        private async Task DisplayAllByWeightAsync()
        {
            var weight = ConsoleHandler.AskForDouble("Enter boundary (max) weight:");
            var parcels = await Service.GetParcelsByWeight(weight);
            ConsoleHandler.PrintCollection(parcels);
        }

        private async Task DisplayAllByCustomerAsync()
        {
            var customerName = ConsoleHandler.AskForString("Enter customer name:",@"^[a-zA-Z]{4,}$");
            var parcels = await Service.GetParcelsByCustomer(customerName);
            ConsoleHandler.PrintCollection(parcels);
        }

        private async Task DisplayAllByTypeAsync()
        {
            var parcelType = ConsoleHandler.AskForEnum<ParcelType>("Enter parcel type:");
            var parcels = await Service.GetParcelsByType(parcelType);
            ConsoleHandler.PrintCollection(parcels);
        }

        private async Task CreateParcelAsync()
        {
            var name = ConsoleHandler.AskForString("Enter name:",@"^[a-zA-Z]{4,}$");
            var description = ConsoleHandler.AskForString("Enter description:",@"^[a-zA-Z]{4,}$");
            var destination = ConsoleHandler.AskForPostalCode("Enter destination (e.g 61072):");
            var weight = ConsoleHandler.AskForDouble("Enter weight:");
            var parcelType = ConsoleHandler.AskForEnum<ParcelType>("Enter parcel type:");
            var customerName = ConsoleHandler.AskForString("Enter customer name:",@"^[a-zA-Z]{4,}$");
            var customerSurname = ConsoleHandler.AskForString("Enter customer surname:",@"^[a-zA-Z]{4,}$");
            
            Console.Clear();
            
            var result = await Service.CreateParcel(name, description, destination, weight, parcelType, customerName, customerSurname);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Parcel created successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

        private async Task DeleteParcelAsync()
        {
            var name = ConsoleHandler.AskForString("Enter name:",@"^[a-zA-Z]{4,}$");
            
            Console.Clear();
            
            var result = await Service.DeleteParcel(name);
            
            if (result.IsSuccessful)
            {
                ConsoleHandler.RaiseSuccess("Parcel created successfully.");
            }
            else
            {
                ConsoleHandler.RaiseError(result.Message);
            }
        }

    }
}