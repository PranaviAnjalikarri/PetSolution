namespace PetSolution1.CommonUtilities
{
    public class Routes
    {
        public const string CreateEmployee = "CreateEmployee";
        public const string UpdateEmployee = "UpdateEmployee/{id}/{partitionKey}";
        public const string GetEmployeeById = "GetEmplyoeeById/{id}/{partitionKey}";
        public const string GetAllEmployees = "GetAllEmployees";
        public const string DeleteEmployeeById = "DeleteEmployeeById/{id}/{partitionKey}";

    }
}
