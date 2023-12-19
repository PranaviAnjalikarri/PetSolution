using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetSolution1.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSolution1.Domain.Interface
{
    public interface IEmployeeDomain
    {
        public Task<IActionResult> CreateEmployeeAsync(HttpRequest req, ILogger log);
        public Task<IActionResult> UpdateEmployeeAsync(HttpRequest req, string id, ILogger log);
        //public Task<IActionResult> UpdateEmployeeAsync(HttpRequest req, Employee employee, string id, ILogger log);
        public Task<IActionResult> DeleleEmployeeByIdAsync(HttpRequest req, string id, string partitionKey, ILogger log);
        public Task<IActionResult> GetAllEmployeesAsync(HttpRequest req, ILogger log);
        public Task<IActionResult> GetEmployeeByIdAsync(HttpRequest req,string id, string partitionKey, ILogger log);
    }
}
