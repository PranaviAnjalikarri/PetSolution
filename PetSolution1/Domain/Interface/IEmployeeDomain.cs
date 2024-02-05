using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetSolution1.Domain.Interface
{
    public interface IEmployeeDomain
    {
        public Task<IActionResult> CreateEmployeeAsync(HttpRequestMessage req, ILogger log);
        public Task<IActionResult> UpdateEmployeeAsync(HttpRequestMessage req, string id, ILogger log);
        public Task<IActionResult> DeleleEmployeeByIdAsync(HttpRequestMessage req, string id, string partitionKey, ILogger log);
        public Task<IActionResult> GetAllEmployeesAsync(HttpRequestMessage req, ILogger log);
        public Task<IActionResult> GetEmployeeByIdAsync(HttpRequestMessage req,string id, string partitionKey, ILogger log);
    }
}
