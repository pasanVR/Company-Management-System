using CompanyManagementSystem.Models;

namespace CompanyManagementSystem.IServices
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetEmployees();
        Employee GetEmployeeById(int id);
        bool AddEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(int id);
    }
}
