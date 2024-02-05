using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;
using PetSolution1;
using PetSolution1.Domain;
using PetSolution1.DAL;

[assembly: WebJobsStartup(typeof(Startup))]
namespace PetSolution1
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddDomainCollection();
            builder.Services.AddDALCollection();
            builder.AddAzureStorageCoreServices();
            builder.AddTimers();
            
        }
    }
}
