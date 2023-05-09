using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using UI.ConsoleHandlers;
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
                { "3", DisplayAllByWeightAsync },
                { "4", DisplayAllByDestinationAsync },
                { "5", CreateParcelAsync },
                { "6", DeleteParcelAsync },
            };

            while (true)
            {
                ConsoleHandler.Clear();
                ConsoleHandler.PrintCaption(@"
                                ██████╗  █████╗ ██████╗  ██████╗███████╗██╗     ███████╗
                                ██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔════╝██║     ██╔════╝
                                ██████╔╝███████║██████╔╝██║     █████╗  ██║     ███████╗
                                ██╔═══╝ ██╔══██║██╔══██╗██║     ██╔══╝  ██║     ╚════██║
                                ██║     ██║  ██║██║  ██║╚██████╗███████╗███████╗███████║
                                ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚══════╝╚══════╝╚══════╝
                                        ");
                ConsoleHandler.PrintInfo(" |=> 1. Display all parcels");
                ConsoleHandler.PrintInfo(" |=> 2. Display all parcels by type");
                ConsoleHandler.PrintInfo(" |=> 3. Display all parcels by weight");
                ConsoleHandler.PrintInfo(" |=> 4. Display all parcels by destination");
                ConsoleHandler.PrintInfo(" |=> 5. Create a new parcel");
                ConsoleHandler.PrintInfo(" |=> 6. Delete a parcel");
                ConsoleHandler.PrintInfo(" <= 9. Back to Main Menu\n");;

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

        private async Task DisplayAllParcelsAsync()
        {
            var parcels = await Service.GetAll();
            ConsoleHandler.PrintCollection(parcels);
        }
        
        private async Task DisplayAllByDestinationAsync()
        {
            var destination = ConsoleHandler.AskForPostalCode("Enter destination (e.g 61072):");
            var parcels = await Service.GetParcelsByDestination(destination);
            ConsoleHandler.PrintCollection(parcels);
        }

        private async Task DisplayAllByWeightAsync()
        {
            var weight = ConsoleHandler.AskForDouble("Enter boundary (max) weight:");
            var parcels = await Service.GetParcelsByWeight(weight);
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
            var description = ConsoleHandler.AskForString("Enter description (min 3 words):",@"^(\w+\s){2,}\w+$");
            var destination = ConsoleHandler.AskForPostalCode("Enter destination (e.g 61072):");
            var weight = ConsoleHandler.AskForDouble("Enter weight:");
            var parcelType = ConsoleHandler.AskForEnum<ParcelType>("Enter parcel type:");
            
            ConsoleHandler.Clear();
            
            var result = await Service.CreateParcel(name, description, destination, weight, parcelType);
            
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
            
            ConsoleHandler.Clear();
            
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