using Microsoft.Extensions.DependencyInjection;
using PetSolution1.Domain.Interface;

namespace PetSolution1.Domain
{
    public static  class ServiceExtension
    {
        public static void AddDomainCollection(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeDomain, EmployeeDomain>();
        }
    }
}
