using CSharpVitamins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetSolution1.CommonUtilities;
using PetSolution1.DAL.Interface;
using PetSolution1.Domain.Interface;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetSolution1.Domain
{
    public class EmployeeDomain : IEmployeeDomain
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _log;
        private readonly IEmployeeDAL _employeeDAL;

        public EmployeeDomain(IConfiguration configuration, ILogger<EmployeeDomain> log, IEmployeeDAL employeeDAL)
        {
            _configuration = configuration;
            _log = log;
            _employeeDAL = employeeDAL;
        }

        public async Task<IActionResult> CreateEmployeeAsync(HttpRequestMessage req,Employee newEmployee, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                newEmployee.Id = ShortGuid.NewGuid();
                log.LogInformation($"Employee details: {newEmployee.Id}, {newEmployee.Name},{newEmployee.Age},{newEmployee.DOB},{newEmployee.Gender},{newEmployee.Email}, {newEmployee.PhoneNumber}");
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
                await _employeeDAL.CreateEmployeeAsync(newEmployee);
                return new OkObjectResult($"Employee created successfully");
            }
            catch (Exception e)
            {
                _log.LogError($"Error in CreateEmployeeAsync: {e.Message}");
                return new ContentResult() { Content = e.Message, StatusCode = 500 };
            }
        }
        public async Task<IActionResult> UpdateEmployeeAsync(HttpRequestMessage req, Employee updatedEmployee, string id, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await req.Content.ReadAsStringAsync();
            try
            {
                log.LogInformation($"Updated Employee details: {updatedEmployee.Id},{updatedEmployee.Name},{updatedEmployee.Age},{updatedEmployee.DOB},{updatedEmployee.Gender},{updatedEmployee.Email},{updatedEmployee.PhoneNumber}");
                if (!(!string.IsNullOrWhiteSpace(updatedEmployee.Name) && IsValidName(updatedEmployee.Name)))
                {
                    return new BadRequestObjectResult("Please give a valid name");
                }
                //Finds the age based on dateofbirth
                updatedEmployee.Age = DateTime.Now.AddYears(-updatedEmployee.DOB.Year).Year;
                //Returns badrequest if the phone number is not valid.
                if (!(!string.IsNullOrWhiteSpace(updatedEmployee.PhoneNumber) && IsValidPhoneNumber(updatedEmployee.PhoneNumber)))
                {
                    return new BadRequestObjectResult("Please give a valid phone number");
                }
                //Returns badrequest if the email is not valid.
                if (!(!string.IsNullOrWhiteSpace(updatedEmployee.Email) && IsValidEmail(updatedEmployee.Email)))
                {
                    return new BadRequestObjectResult("Please give a valid email address");
                }
                
                await _employeeDAL.UpdateEmployeeAsync(updatedEmployee, id);
                // Return an appropriate response
                return new OkObjectResult($"Employee updated successfully");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in UpdateEmployeeAsync: {ex.Message}");
                return new ContentResult() { Content = ex.Message, StatusCode = 500 };
            }
        }
        public async Task<IActionResult> GetAllEmployeesAsync(HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("c# http trigger function processed a request.");
            try
            {
                var employees = await _employeeDAL.GetAllEmployeesAsync();
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in GetAllEmployeesAsync: {ex.Message}");
                return new BadRequestObjectResult("Error getting employees.");
            }
        }

        public async Task<IActionResult> GetEmployeeByIdAsync(HttpRequestMessage req, string id, string partitionKey, ILogger log)
        {
            log.LogInformation("c# http trigger function processed a request.");
            try
            {
                // Check if id is null
                if (string.IsNullOrEmpty(id))
                {
                    return new BadRequestObjectResult("id required.");
                }

                var employees = await _employeeDAL.GetEmployeeByIdAsync(id, partitionKey);
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in GetEmployeeByIdAsync: {ex.Message}");
                return new BadRequestObjectResult("Error getting employee by id.");
            }
        }

        public async Task<IActionResult> DeleleEmployeeByIdAsync(HttpRequestMessage req, string id, string partitionKey, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                // Check if id is null
                if (string.IsNullOrEmpty(id))
                {
                    return new BadRequestObjectResult("Id is required for delete operation.");
                }

                await _employeeDAL.DeleleEmployeeByIdAsync(id, partitionKey);
                return new OkObjectResult("Employee deleted successfully");
            }

            catch (Exception ex)
            {
                _log.LogError($"Error in GetAllEmployeesAsync: {ex.Message}");
                return new BadRequestObjectResult("Error while deleting employee.");
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
    }

}