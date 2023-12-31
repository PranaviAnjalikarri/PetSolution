using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PetSolution1.CommonUtilities;
using System.Threading.Tasks;
using PetSolution1.Domain.Interface;



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
        public async Task<IActionResult> CreateEmployee([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.POST, Route = Routes.CreateEmployee)] HttpRequest req, ILogger log)
        {
            return await employeeDomain.CreateEmployeeAsync(req, log);

        }

        [FunctionName(Constant.UpadteEmployee)]
        public async Task<IActionResult> UpdateEmployee([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.PUT, Route = Routes.UpdateEmployee)] HttpRequest req, string id, string partitionkey, ILogger log)
        {
            return await employeeDomain.UpdateEmployeeAsync(req, id, log);
            //return await employeeDomain.UpdateEmployeeAsync(req,employee,id,log);
        }

        [FunctionName(Constant.GetAllEmployee)]
        public async Task<IActionResult> GetAllEmployees([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.GET, Route = Routes.GetAllEmployees)] HttpRequest req, ILogger log)
        {
              return await employeeDomain.GetAllEmployeesAsync(req,log);
        }

        [FunctionName(Constant.GetByIdEmployee)]
        public async Task<IActionResult> GetEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.GET, Route = Routes.GetEmplyeeById)] HttpRequest req, string id, string partitionKey, ILogger log)
        {
             return await employeeDomain.GetEmployeeByIdAsync(req, id, partitionKey, log);
        }

        [FunctionName(Constant.DeleteEmployee)]
        public async Task<IActionResult> DeleteEmployeeById([HttpTrigger(AuthorizationLevel.Function, HttpMethodTypes.DELETE, Route = Routes.DeleteEmployeeById)] HttpRequest req, string id, string partitionKey, ILogger log)
        {
            return await employeeDomain.DeleleEmployeeByIdAsync(req, id, partitionKey, log);
        }

    }

}