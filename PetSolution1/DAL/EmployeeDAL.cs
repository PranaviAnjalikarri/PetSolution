using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using PetSolution1.CommonUtilities;
using PetSolution1.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSolution1.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private string PrimaryConnectionString;
        private string CosmosDbName;
        private string CosmosDbContainerName;
        private CosmosClient cosmosDbClient;
        private Container containerClient;
        private IConfiguration _configuration;
        public EmployeeDAL(IConfiguration configuation)
        {
            _configuration= configuation;
            this.PrimaryConnectionString = _configuration.GetValue<string>("PrimaryConnectionString");
            this.CosmosDbName = _configuration.GetValue<string>("CosmosDbName");
            this.CosmosDbContainerName = _configuration.GetValue<string>("CosmosDbContainerName");
            this.cosmosDbClient= new CosmosClient(this.PrimaryConnectionString);
            this.containerClient = cosmosDbClient.GetContainer(this.CosmosDbName, this.CosmosDbContainerName);
        }
        public async Task<IActionResult> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                await containerClient.CreateItemAsync(employee, new PartitionKey(employee.Id));
                var response = ("Employee Created Successfully");
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        public async Task<IActionResult> UpdateEmployeeAsync(Employee employee, string id)
        {
            try
            {
                var res = await containerClient.ReadItemAsync<Employee>(id, new PartitionKey(employee.Id));

                var existingItem = res.Resource;
                existingItem.Name = employee.Name;
                existingItem.PhoneNumber = employee.PhoneNumber;
                existingItem.DOB = employee.DOB;
                existingItem.Age = employee.Age;
                existingItem.Gender = employee.Gender;
                existingItem.Email = employee.Email;
                var updateRes = await containerClient.ReplaceItemAsync(existingItem, id, new PartitionKey(employee.Id));
                return new OkObjectResult(updateRes.Resource);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        public async Task<IActionResult> DeleleEmployeeByIdAsync(string id, string partitionKey)
        {
            try
            {
                await containerClient.DeleteItemAsync<Employee>(id, new PartitionKey(partitionKey));
                var response = ("Employee deleted Successfully");
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }


        }
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            try
            {
                var iterator = containerClient.GetItemQueryIterator<Employee>();
                List<Employee> employees = new List<Employee>();

                while (iterator.HasMoreResults)
                {
                    FeedResponse<Employee> currentResultSet = await iterator.ReadNextAsync();
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
        public async Task<IActionResult> GetEmployeeByIdAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<Employee> response = await containerClient.ReadItemAsync<Employee>(id, new PartitionKey(partitionKey));
                return new OkObjectResult(response.Resource);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
