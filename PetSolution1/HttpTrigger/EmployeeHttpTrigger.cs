using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using PetSolution1.CommonUtilities;
using PetSolution1.Domain.Interface;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;


namespace PetSolution1.HttpTrigger
{
    public class EmployeeHttpTrigger
    {
        private readonly IEmployeeDomain employeeDomain;

        public EmployeeHttpTrigger(IEmployeeDomain empDomain)
        {
            this.employeeDomain = empDomain;
        }

        [FunctionName(Constant.PostEmployee)]
        public async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.POST, Route = Routes.CreateEmployee)] HttpRequestMessage req,
            ILogger log)
        {
            try
            {
                string requestBody = await req.Content.ReadAsStringAsync();
                var newEmployee = JsonConvert.DeserializeObject<Employee>(requestBody);
                return await employeeDomain.CreateEmployeeAsync(req, newEmployee, log);
            }
            catch (Exception ex)
            {
                log.LogError($"Error in CreateEmployee: {ex.Message}");
                return new BadRequestObjectResult($"Error in CreateEmployee: {ex.Message}");
            }
        }

        [FunctionName(Constant.UpadteEmployee)]
        public async Task<IActionResult> UpdateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.PUT, Route = Routes.UpdateEmployee)] HttpRequestMessage req,
            string id, string partitionkey, ILogger log)
        {
            try
            {
                string requestBody = await req.Content.ReadAsStringAsync();
                var updatedEmployee = JsonConvert.DeserializeObject<Employee>(requestBody);
                return await employeeDomain.UpdateEmployeeAsync(req, updatedEmployee, id, log);
            }
            catch (Exception ex)
            {
                log.LogError($"Error in UpdateEmployee: {ex.Message}");
                return new BadRequestObjectResult($"Error in UpdateEmployee: {ex.Message}");
            }
        }

        [FunctionName(Constant.GetAllEmployee)]
        public async Task<IActionResult> GetAllEmployees([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.GET, Route = Routes.GetAllEmployees)] HttpRequestMessage req, ILogger log)
        {
            return await employeeDomain.GetAllEmployeesAsync(req, log);
        }

        [FunctionName(Constant.GetByIdEmployee)]
        public async Task<IActionResult> GetEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.GET, Route = Routes.GetEmployeeById)] HttpRequestMessage req, string id, string partitionKey, ILogger log)
        {
            return await employeeDomain.GetEmployeeByIdAsync(req, id, partitionKey, log);
        }

        [FunctionName(Constant.DeleteEmployee)]
        public async Task<IActionResult> DeleteEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.DELETE, Route = Routes.DeleteEmployeeById)] HttpRequestMessage req, string id, string partitionKey, ILogger log)
        {
            return await employeeDomain.DeleleEmployeeByIdAsync(req, id, partitionKey, log);
        }

    }

}
//Post Man Url's
//http://localhost:7299/api/CreateEmployee
//{
//  "Name": "Anjali",
//	"Age": 24,
//  "DOB": "2000-11-15",
//  "Gender":"Female",
//	"PhoneNumber": "9959868490",
//  "Email": "Anjali@gmail.com"
//}
//http://localhost:7299/api/GetAllEmployees
//http://localhost:7299/api/GetEmplyoeeById/aVdenTkSTESLn9zQ9_cQPQ/aVdenTkSTESLn9zQ9_cQPQ
//http://localhost:7299/api/UpdateEmployee/aVdenTkSTESLn9zQ9_cQPQ/aVdenTkSTESLn9zQ9_cQPQ
//http://localhost:7299/api/DeleteEmployeeById/Q5Ldw67OPU-CPjOBajDE0g/Q5Ldw67OPU-CPjOBajDE0g