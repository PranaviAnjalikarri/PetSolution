using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetSolution1.CommonUtilities;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetSolution1.Domain.Interface
{
    public interface IEmployeeDomain
    {
        Task<IActionResult> CreateEmployeeAsync(HttpRequestMessage req, Employee newEmployee, ILogger log);
        Task<IActionResult> UpdateEmployeeAsync(HttpRequestMessage req, Employee updatedEmployee, string id, ILogger log);
        Task<IActionResult> DeleleEmployeeByIdAsync(HttpRequestMessage req, string id, string partitionKey, ILogger log);
        Task<IActionResult> GetAllEmployeesAsync(HttpRequestMessage req, ILogger log);
        Task<IActionResult> GetEmployeeByIdAsync(HttpRequestMessage req, string id, string partitionKey, ILogger log);
        
    }
}
