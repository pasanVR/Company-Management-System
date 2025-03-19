using CompanyManagementSystem.Models;

namespace CompanyManagementSystem.IServices
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartmentById(int id);
        bool AddDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(int id);
    }
}
