using Microsoft.AspNetCore.Mvc;
using PetSolution1.CommonUtilities;
using System.Threading.Tasks;

namespace PetSolution1.DAL.Interface
{
    public interface IEmployeeDAL
    {
        Task<IActionResult> CreateEmployeeAsync(Employee employee);
        Task<IActionResult> UpdateEmployeeAsync(Employee employee, string id);
        Task<IActionResult> DeleleEmployeeByIdAsync(string id, string partitionKey);
        Task<IActionResult> GetAllEmployeesAsync();
        Task<IActionResult> GetEmployeeByIdAsync(string id, string partitionKey);
    }
}