using Microsoft.AspNetCore.Mvc;
using PetSolution1.CommonUtilities;
using System.Threading.Tasks;

namespace PetSolution1.DAL.Interface
{
    public interface IEmployeeDAL
    {
        public Task<IActionResult> CreateEmployeeAsync(Employee employee);
        public Task<IActionResult> UpdateEmployeeAsync(Employee employee, string id);
        public Task<IActionResult> DeleleEmployeeByIdAsync(string id, string partitionKey);
        public Task<IActionResult> GetAllEmployeesAsync();
        public Task<IActionResult> GetEmployeeByIdAsync(string id, string partitionKey);
    }
}
