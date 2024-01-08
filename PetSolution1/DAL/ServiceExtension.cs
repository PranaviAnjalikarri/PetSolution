using Microsoft.Extensions.DependencyInjection;
using PetSolution1.DAL.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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