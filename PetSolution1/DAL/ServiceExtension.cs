using Microsoft.Extensions.DependencyInjection;
using PetSolution1.DAL.Interface;

namespace PetSolution1.DAL
{
    public static class ServiceExtension
    {
        public static void AddDALCollection(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeDAL, EmployeeDAL>();
        }
    }
}