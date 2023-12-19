using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSolution1.CommonUtilities
{
    public class Routes
    {
        public const string CreateEmployee = "Employees";
        public const string UpdateEmployee = "Employees/{id}/{partitionKey}";
        public const string GetEmplyeeById = "Employees/{id}/{partitionKey}";
        public const string GetAllEmployees = "Employees";
        public const string DeleteEmployeeById = "Employees/{id}/{partitionKey}";

    }
}
