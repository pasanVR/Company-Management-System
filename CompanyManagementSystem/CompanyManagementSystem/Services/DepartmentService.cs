using CompanyManagementSystem.Data;
using CompanyManagementSystem.IServices;
using CompanyManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CompanyManagementSystem.Services
{
    // Implementation of IDepartmentService
    public class DepartmentService : IDepartmentService
    {
        private readonly DbService _dbService;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(DbService dbService, ILogger<DepartmentService> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        // Retrieves all departments
        public IEnumerable<Department> GetDepartments()
        {
            var departments = new List<Department>();

            try
            {
                // Using the new DbConnection helper method
                departments = _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("SELECT * FROM Departments", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(new Department
                            {
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                                DepartmentCode = reader["DepartmentCode"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString()
                            });
                        }
                    }
                    return departments;
                });

                _logger.LogInformation("Successfully retrieved departments.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving departments.");
                throw;
            }

            return departments;
        }

        // Retrieves department by ID
        public Department? GetDepartmentById(int id)
        {
            try
            {
                // Using the new DbConnection helper method
                return _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("SELECT * FROM Departments WHERE DepartmentId = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var department = new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            DepartmentName = reader["DepartmentName"].ToString()
                        };

                        _logger.LogInformation($"Department with ID {id} retrieved successfully.");
                        return department;
                    }

                    _logger.LogWarning($"Department with ID {id} not found.");
                    return null;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving department with ID {id}.");
                throw;
            }
        }

        // Adds a new department
        public bool AddDepartment(Department department)
        {
            try
            {
                // Using the new DbConnection helper method
                return _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("INSERT INTO Departments (DepartmentCode, DepartmentName) VALUES (@code, @name)", conn);
                    cmd.Parameters.AddWithValue("@code", department.DepartmentCode);
                    cmd.Parameters.AddWithValue("@name", department.DepartmentName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation($"Department '{department.DepartmentName}' added successfully.");
                        return true;
                    }

                    _logger.LogWarning($"Failed to add department '{department.DepartmentName}'. No rows affected.");
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding department '{department.DepartmentName}'");
                throw;
            }
        }

        // Updates an existing department
        public bool UpdateDepartment(Department department)
        {
            try
            {
                // Using the new DbConnection helper method
                return _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("UPDATE Departments SET DepartmentCode = @code, DepartmentName = @name WHERE DepartmentId = @id", conn);
                    cmd.Parameters.AddWithValue("@code", department.DepartmentCode);
                    cmd.Parameters.AddWithValue("@name", department.DepartmentName);
                    cmd.Parameters.AddWithValue("@id", department.DepartmentId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation($"Department ID {department.DepartmentId} updated successfully.");
                        return true;
                    }

                    _logger.LogWarning($"Failed to update department ID {department.DepartmentId}. No rows affected.");
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating department ID {department.DepartmentId}");
                throw;
            }
        }

        // Deletes a department by ID
        public bool DeleteDepartment(int id)
        {
            try
            {
                // Using the new DbConnection helper method
                return _dbService.DbConnection(conn =>
                {
                    var cmd = new SqlCommand("DELETE FROM Departments WHERE DepartmentId = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation($"Department ID {id} deleted successfully.");
                        return true;
                    }

                    _logger.LogWarning($"Failed to delete department ID {id}. No rows affected.");
                    return false;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting department ID {id}");
                throw;
            }
        }
    }
}
