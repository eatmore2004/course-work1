using UI.ConsoleManagers;

namespace UI
{
    public class AppManager
    {
        private readonly OrderConsoleManager _orderConsoleManager;
        private readonly StaffConsoleManager _staffConsoleManager;
        private readonly ParcelConsoleManager _parcelConsoleManager;
        private readonly CustomerConsoleManager _customerConsoleManager;

        public AppManager(OrderConsoleManager orderConsoleManager,
            StaffConsoleManager staffConsoleManager,
            ParcelConsoleManager parcelConsoleManager,
            CustomerConsoleManager customerConsoleManager)
        {
            _orderConsoleManager = orderConsoleManager;
            _staffConsoleManager = staffConsoleManager;
            _parcelConsoleManager = parcelConsoleManager;
            _customerConsoleManager = customerConsoleManager;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                Console.Clear();
                ConsoleHandler.PrintCaption("Main Menu");
                ConsoleHandler.PrintInfo(" => 1. Staff operations");
                ConsoleHandler.PrintInfo(" => 2. Parcel operations");
                ConsoleHandler.PrintInfo(" => 3. Order operations");
                ConsoleHandler.PrintInfo(" => 4. Customer operations");
                ConsoleHandler.PrintInfo(" => 5. Exit\n");
                ConsoleHandler.Print("Enter the operation number: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await _staffConsoleManager.PerformOperationsAsync();
                        break;
                    case "2":
                        await _parcelConsoleManager.PerformOperationsAsync();
                        break;
                    case "3":
                        await _orderConsoleManager.PerformOperationsAsync();
                        break;
                    case "4":
                        await _customerConsoleManager.PerformOperationsAsync();
                        break;
                    case "5":
                        Console.Clear();
                        return;
                    default:
                        ConsoleHandler.RaiseError("Invalid operation number.");
                        break;
                }
            }
        }
    }
}