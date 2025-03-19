using CompanyManagementSystem.IServices;
using CompanyManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        //Get all employees.
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all employees.");

                var employees = _employeeService.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employees.");
                return StatusCode(500, "An error occurred while fetching employees.");
            }
        }

        //Get employee by ID.
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching employee with ID: {EmployeeId}", id);

                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    return NotFound($"Employee with ID {id} not found.");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the employee with ID: {EmployeeId}", id);
                return StatusCode(500, "An error occurred while fetching the employee.");
            }
        }

        //Create a new employee.
        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    _logger.LogWarning("Employee data is null.");
                    return BadRequest("Employee data is null.");
                }

                _logger.LogInformation("Creating a new employee: {EmployeeName}", employee.FirstName + " " + employee.LastName);

                var isCreated = _employeeService.AddEmployee(employee);
                if (!isCreated)
                {
                    _logger.LogWarning("Failed to create employee: {EmployeeName}", employee.FirstName + " " + employee.LastName);
                    return StatusCode(500, "Failed to create employee.");
                }

                _logger.LogInformation("Employee created successfully: {EmployeeName}", employee.FirstName + " " + employee.LastName);
                return Ok("Employee created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employee: {EmployeeName}", employee.FirstName + " " + employee.LastName);
                return StatusCode(500, "An error occurred while creating the employee.");
            }
        }

        //Update an existing employee.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null || employee.EmployeeId != id)
                {
                    _logger.LogWarning("Invalid employee data for ID: {EmployeeId}", id);
                    return BadRequest("Invalid employee data.");
                }

                _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

                var existingEmployee = _employeeService.GetEmployeeById(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    return NotFound($"Employee with ID {id} not found.");
                }

                var isUpdated = _employeeService.UpdateEmployee(employee);
                if (!isUpdated)
                {
                    _logger.LogWarning("Failed to update employee with ID: {EmployeeId}", id);
                    return StatusCode(500, "Failed to update employee.");
                }

                _logger.LogInformation("Employee with ID {EmployeeId} updated successfully.", id);
                return Ok("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating employee with ID: {EmployeeId}", id);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        //Delete employee by ID.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

                var existingEmployee = _employeeService.GetEmployeeById(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found.", id);
                    return NotFound($"Employee with ID {id} not found.");
                }

                var isDeleted = _employeeService.DeleteEmployee(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete employee with ID: {EmployeeId}", id);
                    return StatusCode(500, "Failed to delete employee.");
                }

                _logger.LogInformation("Employee with ID {EmployeeId} deleted successfully.", id);
                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee with ID: {EmployeeId}", id);
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }
    }
}
