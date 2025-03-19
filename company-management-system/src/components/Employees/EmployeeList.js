import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../../services/api';

export default function EmployeeList() {
  const [employees, setEmployees] = useState([]);
  const [departments, setDepartments] = useState([]);

  useEffect(() => {
    fetchEmployees();
    fetchDepartments();
  }, []);

  const fetchEmployees = async () => {
    try {
      const response = await api.get('/employee');
      setEmployees(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  const fetchDepartments = async () => {
    try {
      const response = await api.get('/department');
      setDepartments(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  const deleteEmployee = async (id) => {
    if (!window.confirm('Delete this employee?')) return;
    try {
      await api.delete(`/employee/${id}`);
      fetchEmployees();
    } catch (error) {
      console.error(error);
    }
  };

  const getDepartmentName = (departmentId) => {
    const department = departments.find(dept => dept.departmentId === departmentId);
    return department ? department.departmentName : 'Department is not found';
  };

  return (
    <div>
      <h2>Employees</h2>
      <Link to="/employees/add"><button>Add Employee</button></Link>
      <table border="1" cellPadding="8" style={{ marginTop: '10px', width: '100%' }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>DOB</th>
            <th>Age</th>
            <th>Salary</th>
            <th>Department</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {employees.map((emp) => (
            <tr key={emp.employeeId}>
              <td>{emp.employeeId}</td>
              <td>{emp.firstName} {emp.lastName}</td>
              <td>{emp.email}</td>
              <td>{new Date(emp.dateOfBirth).toLocaleDateString()}</td>
              <td>{emp.age}</td>
              <td>{emp.salary}</td>
              <td>{getDepartmentName(emp.departmentId)}</td>
              <td>
                <Link to={`/employees/edit/${emp.employeeId}`}><button>Edit</button></Link>
                <button onClick={() => deleteEmployee(emp.employeeId)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
