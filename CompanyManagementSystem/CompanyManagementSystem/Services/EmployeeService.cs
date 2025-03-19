using CompanyManagementSystem.Data;
using CompanyManagementSystem.IServices;
using CompanyManagementSystem.Models;
using System.Data.SqlClient;

namespace CompanyManagementSystem.Services
{
    // Implementation of IEmployeeService.
    public class EmployeeService : IEmployeeService
    {
        private readonly DbService _dbService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(DbService dbService, ILogger<EmployeeService> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        // Retrieves all employees from the database.
        public IEnumerable<Employee> GetEmployees()
        {
            try
            {
                _logger.LogInformation("Fetching all employees.");

                return _dbService.DbConnection(conn =>
                {
                    var employees = new List<Employee>();

                    var cmd = new SqlCommand("SELECT * FROM Employees", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Age = Convert.ToInt32(reader["Age"]),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"])
                            });
                        }
                    }

                    _logger.LogInformation("Successfully fetched {EmployeeCount} employees.", employees.Count);
                    return employees;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees.");
                throw;
            }
        }

        // Retrieves an employee by their unique ID.
        public Employee? GetEmployeeById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching employee with ID: {EmployeeId}", id);

                return _dbService.DbConnection(conn =>
                {
                    Employee employee = null;

                    var cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeId = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Age = Convert.ToInt32(reader["Age"]),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"])
                            };
                        }
                    }

                    _logger.LogInformation("Successfully fetched employee with ID: {EmployeeId}", id);
                    return employee;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with ID: {EmployeeId}", id);
                throw;
            }
        }

        // Adds a new employee to the database.
        public bool AddEmployee(Employee employee)
        {
            try
            {
                _logger.LogInformation("Adding new employee: {EmployeeName}", $"{employee.FirstName} {employee.LastName}");

                return _dbService.DbConnection(conn =>
                {
                    int age = CalculateAge(employee.DateOfBirth);

                    var cmd = new SqlCommand(@"
                        INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Age, Salary, DepartmentId)
                        VALUES (@firstName, @lastName, @email, @dob, @age, @salary, @departmentId)", conn);

                    cmd.Parameters.AddWithValue("@firstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@email", employee.Email);
                    cmd.Parameters.AddWithValue("@dob", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@departmentId", employee.DepartmentId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation("Employee '{EmployeeName}' added successfully.", $"{employee.FirstName} {employee.LastName}");
                        return true;
                    }

                    _logger.LogWarning("Failed to add employee '{EmployeeName}'. No rows affected.", $"{employee.FirstName} {employee.LastName}");
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding employee: {EmployeeName}", $"{employee.FirstName} {employee.LastName}");
                throw;
            }
        }

        // Updates an existing employee's details in the database.
        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                _logger.LogInformation("Updating employee with ID: {EmployeeId}", employee.EmployeeId);

                return _dbService.DbConnection(conn =>
                {
                    int age = CalculateAge(employee.DateOfBirth);

                    var cmd = new SqlCommand(@"
                        UPDATE Employees 
                        SET FirstName = @firstName,
                            LastName = @lastName,
                            Email = @email,
                            DateOfBirth = @dob,
                            Age = @age,
                            Salary = @salary,
                            DepartmentId = @departmentId
                        WHERE EmployeeId = @id", conn);

                    cmd.Parameters.AddWithValue("@firstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@email", employee.Email);
                    cmd.Parameters.AddWithValue("@dob", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@departmentId", employee.DepartmentId);
                    cmd.Parameters.AddWithValue("@id", employee.EmployeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation("Employee with ID {EmployeeId} updated successfully.", employee.EmployeeId);
                        return true;
                    }

                    _logger.LogWarning("Failed to update employee with ID {EmployeeId}. No rows affected.", employee.EmployeeId);
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with ID: {EmployeeId}", employee.EmployeeId);
                throw;
            }
        }

        // Deletes an employee from the database by their unique ID.
        public bool DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

                return _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation("Employee with ID {EmployeeId} deleted successfully.", id);
                        return true;
                    }

                    _logger.LogWarning("Failed to delete employee with ID {EmployeeId}. No rows affected.", id);
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID: {EmployeeId}", id);
                throw;
            }
        }

        // Calculates the age from DateOfBirth.
        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
