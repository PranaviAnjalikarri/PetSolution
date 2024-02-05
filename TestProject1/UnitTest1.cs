using Moq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using PetSolution1.CommonUtilities;
using PetSolution1.Domain;
using System.Collections;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Options;
using PetSolution1.DAL.Interface;
using Microsoft.Extensions.Configuration;

public class HttptriggersTests
{
    private Mock<IConfiguration> configurationMock;
    private Mock<ILogger<EmployeeDomain>> loggerMock;
    private Mock<IEmployeeDAL> employeeDalMock;
    private HttpRequestMessage httpRequestMessage;

    private void TestSetup()
    {
        this.configurationMock = new Mock<IConfiguration>();
        this.loggerMock = new Mock<ILogger<EmployeeDomain>>();
        this.employeeDalMock = new Mock<IEmployeeDAL>();
        this.httpRequestMessage = new HttpRequestMessage();
    }

    [Theory]
    [ClassData(typeof(EmployeeSuccessData))]
    public async Task CreateEmployee_ReturnsOkObjectResult(Employee mockData)
    {
        //Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(dal => dal.CreateEmployeeAsync(It.IsAny<Employee>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.CreateEmployeeAsync(req, Mock.Of<ILogger>());

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

    }

    [Theory]
    [ClassData(typeof(EmployeeInvalidData))]
    public async Task CreateEmployee_ReturnsBadrequest(Employee mockData)
    {

        //Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(dal => dal.CreateEmployeeAsync(It.IsAny<Employee>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.CreateEmployeeAsync(req, Mock.Of<ILogger>());

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

    }

    [Theory]
    [ClassData(typeof(EmployeeSuccessData))]
    public async Task UpdateEmployeeAsync_ReturnsOkObjectResult(Employee mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.UpdateEmployeeAsync(req, mockData.Id, loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeInvalidData))]
    public async Task UpdateEmployeeAsync_ReturnsBadRequestResult(Employee mockData)
    {

        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.UpdateEmployeeAsync(req, mockData.Id, loggerMock.Object);
        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeData))]
    public async Task GetAllEmployeesAsync_ReturnsOkResult(List<Employee> mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.GetAllEmployeesAsync());
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.GetAllEmployeesAsync(req, loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeSuccessData))]
    public async Task GetEmployeeByIdAsync_ReturnOkObjectResult(Employee mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.GetEmployeeByIdAsync(It.IsAny<string>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.GetEmployeeByIdAsync(req, mockData.Id, It.IsAny<string>(), loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeEmptyId))]
    public async Task GetEmployeeByIdAsync_ReturnsBadRequestResult(Employee mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.GetEmployeeByIdAsync(It.IsAny<string>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.GetEmployeeByIdAsync(req, mockData.Id, It.IsAny<string>(), loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeSuccessData))]
    public async Task DeleteEmployeeByIdAsync_ReturnsOkResult(Employee mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.DeleleEmployeeByIdAsync(It.IsAny<string>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.DeleleEmployeeByIdAsync(req, mockData.Id, It.IsAny<string>(), loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Theory]
    [ClassData(typeof(EmployeeEmptyId))]
    public async Task DeleteEmployeeByIdAsync_ReturnsBadRequestResult(Employee mockData)
    {
        // Arrange
        var req = CreateMockRequest(mockData);
        TestSetup();
        employeeDalMock.Setup(e => e.DeleleEmployeeByIdAsync(It.IsAny<string>(), It.IsAny<string>()));
        var employeeDomain = new EmployeeDomain(configurationMock.Object, loggerMock.Object, employeeDalMock.Object);

        // Act
        dynamic result = await employeeDomain.DeleleEmployeeByIdAsync(req, mockData.Id, It.IsAny<string>(), loggerMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    public class EmployeeSuccessData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 21, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class EmployeeInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Employee { Id = string.Empty, Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 21, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = string.Empty, DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 21, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(), Gender = "Female", Age = 21, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = string.Empty, Age = 21, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 0, Email = "anjali@gmail.com", PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 21, Email = string.Empty, PhoneNumber = "9959868490" } };
            yield return new object[] { new Employee { Id = "1", Name = "Anjali", DOB = new DateOnly(2000, 11, 15), Gender = "Female", Age = 21, Email = "anjali@gmail.com", PhoneNumber = string.Empty } };
            yield return new object[] { new Employee { Id = string.Empty, Name = string.Empty, DOB = new DateOnly(), Gender = string.Empty, Age = 0, Email = string.Empty, PhoneNumber = string.Empty } };

        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class EmployeeEmptyId : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Employee { Id = string.Empty } };

        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class EmployeeData : TheoryData<List<Employee>>
    {
        public EmployeeData()
        {
            // Add test data with a list of employees
            Add(new List<Employee>
        {
            new Employee { Id = "1", Name = "John Doe", DOB =  new DateOnly(2000, 11, 15),Gender = "Male", Age = 21,PhoneNumber = "1234567890", Email = "john.doe@example.com" },
            new Employee { Id = "2", Name = "Anjali", DOB =  new DateOnly(2000, 11, 15),Gender = "Female" ,Age = 22, PhoneNumber ="9876543210", Email = "jane.doe@example.com" }
            // Add more employees as needed
        });
        }
    }
    public static HttpRequestMessage CreateMockRequest(object body)
    {
        var req = new HttpRequestMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
        };
        req = SetupHttp(req);
        return req;
    }
    public static HttpRequestMessage SetupHttp(HttpRequestMessage requestMessage)
    {
        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        var formatter = new XmlMediaTypeFormatter();
        var context = new DefaultHttpContext();

        var contentNegotiator = new Mock<IContentNegotiator>();
        contentNegotiator
            .Setup(c => c.Negotiate(It.IsAny<Type>(), It.IsAny<HttpRequestMessage>(), It.IsAny<IEnumerable<MediaTypeFormatter>>()))
            .Returns(new ContentNegotiationResult(formatter, mediaType: null));

        var options = new WebApiCompatShimOptions();

        if (formatter == null)
        {
            options.Formatters.AddRange(new MediaTypeFormatterCollection());
        }
        else
        {
            options.Formatters.Add(formatter);
        }

        var optionsAccessor = new Mock<IOptions<WebApiCompatShimOptions>>();
        optionsAccessor.SetupGet(o => o.Value).Returns(options);

        services.Setup(s => s.GetService(typeof(IOptions<WebApiCompatShimOptions>))).Returns(optionsAccessor.Object);

        if (contentNegotiator != null)
        {
            services.Setup(s => s.GetService(typeof(IContentNegotiator))).Returns(contentNegotiator);
        }

        context.RequestServices = CreateServices(contentNegotiator.Object, formatter);
        requestMessage.Options.TryAdd(nameof(HttpContext), context);
        return requestMessage;
    }

    private static IServiceProvider CreateServices(IContentNegotiator contentNegotiator = null,MediaTypeFormatter formatter = null)
    {
        var options = new WebApiCompatShimOptions();

        if (formatter == null)
        {
            options.Formatters.AddRange(new MediaTypeFormatterCollection());
        }
        else
        {
            options.Formatters.Add(formatter);
        }

        var optionsAccessor = new Mock<IOptions<WebApiCompatShimOptions>>();
        optionsAccessor.SetupGet(o => o.Value).Returns(options);

        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        services.Setup(s => s.GetService(typeof(IOptions<WebApiCompatShimOptions>))).Returns(optionsAccessor.Object);

        if (contentNegotiator != null)
        {
            services.Setup(s => s.GetService(typeof(IContentNegotiator))).Returns(contentNegotiator);
        }

        return services.Object;
    }

}

