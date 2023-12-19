using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PetSolution1
{
    public static class Function1
    {
        private static readonly string CosmosDBAccountUri = "https://localhost:8081/";
        private static readonly string CosmosDBAccountPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string CosmosDbName = "EmployeeManagementDB";
        private static readonly string CosmosDbContainerName = "Employees";
        private static CosmosClient cosmosDbClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
        private static Container containerClient = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
        
        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Employees")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

             var  employeeId = Guid.NewGuid().ToString();
            
            string queryData = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            try
            {
                Employee employee = new Employee();
                queryData = queryData ?? data?.name;
                employee.Id = employeeId;
                //Call IsValidName Method
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidName(queryData))
                {
                    employee.Name = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid name");
                }
                queryData = req.Query["dob"];
                queryData = queryData ?? data?.dob;
                //Check Whether date format is correct or not and calculates actual age
                try
                {
                    DateOnly dob = DateOnly.ParseExact(queryData, "dd/MM/yyyy");
                    employee.Age = DateTime.Now.AddYears(-dob.Year).Year;
                }
                catch (Exception e)
                {
                    return new BadRequestObjectResult("Please give a valid date in this format(dd/MM/yyyy)");
                }
                queryData = req.Query["phonenumber"];
                queryData = queryData ?? data?.phonenumber;
                //Check Whether phone number is correct or not
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidPhoneNumber(queryData))
                {
                    employee.PhoneNumber = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid phone number");
                }
                queryData = req.Query["email"];
                queryData = queryData ?? data?.email;
                //Check Whether email is correct or not
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidEmail(queryData))
                {
                    employee.Email = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid email address");
                }

                try
                {
                    var response = await containerClient.CreateItemAsync(employee, new PartitionKey(employee.Name));
                    return new OkObjectResult(response);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }

            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
        [FunctionName("UpdateEmployee")]
        public static async Task<IActionResult> UpdateEmployee([HttpTrigger(AuthorizationLevel.Function, "put", Route = "Employees/{id}/{partitionKey}")] HttpRequest req, string id, string partitionKey, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            
            string queryData = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            try
            {
                Employee employee = new Employee();
                queryData = queryData ?? data?.name;
                //Call IsValidName Method
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidName(queryData))
                {
                    employee.Name = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid name");
                }
                queryData = req.Query["dob"];
                queryData = queryData ?? data?.dob;
                
                //Check Whether date format is correct or not and calculates actual age
                try
                {
                    DateOnly dob = DateOnly.ParseExact(queryData, "dd/MM/yyyy");
                    employee.Age = DateTime.Now.AddYears(-dob.Year).Year;
                }
                catch (Exception e)
                {
                    return new BadRequestObjectResult("Please give a valid date in this format(dd/MM/yyyy)");
                }
                queryData = req.Query["phonenumber"];
                queryData = queryData ?? data?.phonenumber;
                //Check Whether phone number is correct or not
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidPhoneNumber(queryData))
                {
                    employee.PhoneNumber = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid phone number");
                }
                queryData = req.Query["email"];
                queryData = queryData ?? data?.email;
                //Check Whether email is correct or not
                if (!string.IsNullOrWhiteSpace(queryData) && IsValidEmail(queryData))
                {
                    employee.Email = queryData;
                }
                else
                {
                    return new BadRequestObjectResult("Please give a valid email address");
                }


                try
                {
                   var res = await containerClient.ReadItemAsync<Employee>(id, new PartitionKey(employee.Name));

                    var existingItem = res.Resource;
                    existingItem.Name = employee.Name;
                    existingItem.PhoneNumber = employee.PhoneNumber;
                    existingItem.DOB = employee.DOB;
                    existingItem.Age = employee.Age;
                    existingItem.Email = employee.Email;
                    var updateRes = await containerClient.ReplaceItemAsync(existingItem, id, new PartitionKey(employee.Name));
                    return new OkObjectResult(updateRes.Resource);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }

            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
        

        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Employees/{id?}/{partitionKey?}")] HttpRequest req, string id, string partitionKey,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (id == null && partitionKey == null)
            {
                try
                {
                    var sqlQuery = "SELECT * FROM c";
                    QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                    FeedIterator<Employee> queryResultSetIterator = containerClient.GetItemQueryIterator<Employee>(queryDefinition);
                    List<Employee> employees = new List<Employee>();
                    while (queryResultSetIterator.HasMoreResults)
                    {
                        FeedResponse<Employee> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                        foreach (Employee employee in currentResultSet)
                        {
                            employees.Add(employee);
                        }
                    }
                    return new OkObjectResult(employees);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }
            }
            else
            {
                try
                {
                    ItemResponse<Employee> response = await containerClient.ReadItemAsync<Employee>(id,new PartitionKey(partitionKey));
                    return new OkObjectResult(response.Resource);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }
            }
        }

        [FunctionName("DeleteEmployee")]
        public static async Task<IActionResult> DeleteEmployee(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Employees/{id}/{partitionKey}")] HttpRequest req, string id,string partitionKey, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var response = await containerClient.DeleteItemAsync<Employee>(id, new PartitionKey(partitionKey));
                return new OkObjectResult(response.StatusCode);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message);
            }
        }


        //Return true if valid else false
        private static bool IsValidName(string name)
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
    }
    public class Employee
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("dob")]
        public DateOnly DOB { get; set; }= new DateOnly();

        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }
}
