using BLL.Abstractions.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IParcelService, ParcelService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IStaffService, StaffService>();
            
            DAL.DependencyRegistration.RegisterRepositories(services);
        }
    }
}

