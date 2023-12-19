using Microsoft.Extensions.DependencyInjection;
using PetSolution1.Domain.Interface;
using Microsoft.Azure.WebJobs.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
