using CompanyManagementSystem.IServices;
using CompanyManagementSystem.Models;
using CompanyManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        //Get all departments.
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all departments.");

                var departments = _departmentService.GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching departments.");
                return StatusCode(500, "An error occurred while fetching departments.");
            }
        }

        //Get department by ID.
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching department with ID: {DepartmentId}", id);

                var department = _departmentService.GetDepartmentById(id);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found.", id);
                    return NotFound($"Department with ID {id} not found.");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the department with ID: {DepartmentId}", id);
                return StatusCode(500, "An error occurred while fetching the department.");
            }
        }

        //Create a new department.
        [HttpPost]
        public IActionResult Create([FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    _logger.LogWarning("Department data is null.");
                    return BadRequest("Department data is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating a new department: {DepartmentName}", department.DepartmentName);

                var isCreated = _departmentService.AddDepartment(department);
                if (!isCreated)
                {
                    _logger.LogWarning("Failed to create department: {DepartmentName}", department.DepartmentName);
                    return StatusCode(500, "Failed to create department.");
                }

                _logger.LogInformation("Department created successfully: {DepartmentName}", department.DepartmentName);
                return StatusCode(201,"Department created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the department: {DepartmentName}", department.DepartmentName);
                return StatusCode(500, "An error occurred while creating the department.");
            }
        }

        //Update an existing department.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Department department)
        {
            try
            {
                if (department == null || department.DepartmentId != id)
                {
                    _logger.LogWarning("Invalid department data for ID: {DepartmentId}", id);
                    return BadRequest("Invalid department data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating department with ID: {DepartmentId}", id);

                var existingDepartment = _departmentService.GetDepartmentById(id);
                if (existingDepartment == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found.", id);
                    return NotFound($"Department with ID {id} not found.");
                }

                var isUpdated = _departmentService.UpdateDepartment(department);
                if (!isUpdated)
                {
                    _logger.LogWarning("Failed to update department with ID: {DepartmentId}", id);
                    return StatusCode(500, "Failed to update department.");
                }

                _logger.LogInformation("Department with ID {DepartmentId} updated successfully.", id);
                return Ok("Department updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating department with ID: {DepartmentId}", id);
                return StatusCode(500, "An error occurred while updating the department.");
            }
        }

        //Delete department by ID.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);

                var existingDepartment = _departmentService.GetDepartmentById(id);
                if (existingDepartment == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found.", id);
                    return NotFound($"Department with ID {id} not found.");
                }

                var isDeleted = _departmentService.DeleteDepartment(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete department with ID: {DepartmentId}", id);
                    return StatusCode(500, "Failed to delete department.");
                }

                _logger.LogInformation("Department with ID {DepartmentId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting department with ID: {DepartmentId}", id);
                return StatusCode(500, "An error occurred while deleting the department.");
            }
        }
    }
}
