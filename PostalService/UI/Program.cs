using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    internal static class Program
    {
        private static Task Main(string[] args)
        {
            var serviceProvider = DependencyRegistration.Register();

            using (var scope = serviceProvider.CreateScope())
            {
                var appManager = scope.ServiceProvider.GetService<AppManager>();
                appManager?.StartAsync().Wait();
            }

            return Task.CompletedTask;
        }
    }
}