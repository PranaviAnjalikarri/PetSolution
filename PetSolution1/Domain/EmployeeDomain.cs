using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Hosting;
using Newtonsoft.Json;
using PetSolution1.CommonUtilities;
using PetSolution1.DAL;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSharpVitamins;
using PetSolution1.Domain.Interface;


namespace PetSolution1.Domain
{
    public class EmployeeDomain : IEmployeeDomain
    {
        private readonly IConfiguration _configuration;
        public EmployeeDomain(IConfiguration configuation)
        {
            _configuration = configuation;
        }
        public async Task<IActionResult> CreateEmployeeAsync(HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var employeeId = ShortGuid.NewGuid();
            try
            {
                Employee newEmployee = JsonConvert.DeserializeObject<Employee>(requestBody);
                newEmployee.Id = ShortGuid.NewGuid();
                //Returns badrequest if the name is not valid.
                if (!(!string.IsNullOrWhiteSpace(newEmployee.Name) && IsValidName(newEmployee.Name)))
                {
                    return new BadRequestObjectResult("Please give a valid name");
                }
                //Finds the age based on dateofbirth
                newEmployee.Age = DateTime.Now.AddYears(-newEmployee.DOB.Year).Year;
                //Returns badrequest if the phone number is not valid.
                if (!(!string.IsNullOrWhiteSpace(newEmployee.PhoneNumber) && IsValidPhoneNumber(newEmployee.PhoneNumber)))
                {
                    return new BadRequestObjectResult("Please give a valid phone number");
                }
                //Returns badrequest if the email is not valid.
                if (!(!string.IsNullOrWhiteSpace(newEmployee.Email) && IsValidEmail(newEmployee.Email)))
                {
                    return new BadRequestObjectResult("Please give a valid email address");
                }
                var employeeDAL = new EmployeeDAL(_configuration);
                return await employeeDAL.CreateEmployeeAsync(newEmployee);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.Message, StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetAllEmployeesAsync(HttpRequest req, ILogger log)
        {
            log.LogInformation("c# http trigger function processed a request.");
            EmployeeDAL employeeDAL = new EmployeeDAL(_configuration);
            return await employeeDAL.GetAllEmployeesAsync();

        }

        public async Task<IActionResult> GetEmployeeByIdAsync(HttpRequest req, string id, string partitionKey, ILogger log)
        {
            log.LogInformation("c# http trigger function processed a request.");
            EmployeeDAL employeeDAL = new EmployeeDAL(_configuration);
            return await employeeDAL.GetEmployeeByIdAsync(id, partitionKey);
        }

        public async Task<IActionResult> UpdateEmployeeAsync(HttpRequest req, string id, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                Employee newEmployee = JsonConvert.DeserializeObject<Employee>(requestBody);
                //Call IsValidName Method
                if (!(!string.IsNullOrWhiteSpace(newEmployee.Name) && IsValidName(newEmployee.Name)))
                {
                    return new BadRequestObjectResult("Please give a valid name");
                }

                //Finds the age based on dateofbirth
                newEmployee.Age = DateTime.Now.AddYears(-newEmployee.DOB.Year).Year;
                //Returns badrequest if the phone number is not valid.
                if (!(!string.IsNullOrWhiteSpace(newEmployee.PhoneNumber) && IsValidPhoneNumber(newEmployee.PhoneNumber)))
                {
                    return new BadRequestObjectResult("Please give a valid phone number");
                }
                //Returns badrequest if the email is not valid.
                if (!(!string.IsNullOrWhiteSpace(newEmployee.Email) && IsValidEmail(newEmployee.Email)))
                {
                    return new BadRequestObjectResult("Please give a valid email address");
                }
                var employeeDAL = new EmployeeDAL(_configuration);
                return await employeeDAL.UpdateEmployeeAsync(newEmployee,newEmployee.Id);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content=e.Message, StatusCode=500};
            }
        }

        public async Task<IActionResult> DeleteEmployeeByIdAsync(string id, string partitionKey, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var employeeDAL = new EmployeeDAL(_configuration);
                return await employeeDAL.DeleleEmployeeByIdAsync(id, partitionKey);
            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message);
            }
        }

        //Return true if valid else false
        private static bool IsValidEmail(string email)
        {
            try
            {
                // Check whether the phone number is valid or not
                if (Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }
        //Return true if valid else false
        private bool IsValidName(string name)
        {
            try
            {
                //Check whether the name is valid or not
                if (name.Length <= 100 && Regex.IsMatch(name, @"^[a-zA-Z ]+$"))
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }
        //Return true if valid else false
        private static bool IsValidPhoneNumber(string phno)
        {
            try
            {
                // Check whether the phone number is valid or not
                if (phno.Length == 10 && Regex.IsMatch(phno, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public Task<IActionResult> UpdateEmployeeAsync(HttpRequest req, Employee employee, string id, ILogger log)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleleEmployeeByIdAsync(HttpRequest req, string id, string partitionKey, ILogger log)
        {
            throw new NotImplementedException();
        }
    }
    
}