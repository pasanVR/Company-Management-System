import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../services/api';
import '../../styles/employees.css';

export default function EmployeeForm() {
  const [employee, setEmployee] = useState({
    firstName: '',
    lastName: '',
    email: '',
    dateOfBirth: '',
    salary: '',
    departmentId: ''
  });

  const [departments, setDepartments] = useState([]);
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    fetchDepartments();
    if (id) fetchEmployee(id);
  }, [id]);

  const fetchDepartments = async () => {
    try {
      const response = await api.get('/department');
      setDepartments(response.data);
    } catch (error) {
      console.error('Error fetching departments:', error);
    }
  };

  const fetchEmployee = async (id) => {
    try {
      const response = await api.get(`/employee/${id}`);
      const fetchedEmployee = response.data;
  
      // Format the date to YYYY-MM-DD if it exists
      if (fetchedEmployee.dateOfBirth) {
        fetchedEmployee.dateOfBirth = fetchedEmployee.dateOfBirth.split('T')[0];
      }
  
      setEmployee(fetchedEmployee);
    } catch (error) {
      console.error('Error fetching employee:', error);
    }
  };
  

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEmployee({ ...employee, [name]: value });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!employee.firstName) newErrors.firstName = 'First Name is required';
    if (!employee.lastName) newErrors.lastName = 'Last Name is required';
    if (!employee.email) newErrors.email = 'Email is required';
    if (!employee.dateOfBirth) newErrors.dateOfBirth = 'Date of Birth is required';
    if (!employee.salary) newErrors.salary = 'Salary is required';
    if (!employee.departmentId) newErrors.departmentId = 'Department is required';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;
  
    try {
      if (!id) {
        // Only check for duplicate emails when creating a new employee
        const response = await api.get('/employee');
        const existingEmployee = response.data.find(
          (emp) => emp.email === employee.email
        );
  
        if (existingEmployee) {
          alert('An employee with this email already exists!');
          return;
        }
  
        // Create new employee
        await api.post('/employee', employee);
        alert('Employee created!');
      } else {
        // Update existing employee
        await api.put(`/employee/${id}`, employee);
        alert('Employee updated!');
      }
  
      navigate('/employees');
    } catch (error) {
      console.error('Submit error:', error.response?.data || error.message);
      alert('An error occurred. Please try again.');
    }
  };

  return (
    <div>
      <h2>{id ? 'Edit Employee' : 'Add Employee'}</h2>
      <form onSubmit={handleSubmit} className="employee-form">
        <div className="form-group">
          <label>First Name</label>
          <input
            name="firstName"
            value={employee.firstName}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.firstName && <span className="error">{errors.firstName}</span>}
        </div>

        <div className="form-group">
          <label>Last Name</label>
          <input
            name="lastName"
            value={employee.lastName}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.lastName && <span className="error">{errors.lastName}</span>}
        </div>

        <div className="form-group">
          <label>Email</label>
          <input
            name="email"
            type="email"
            value={employee.email}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.email && <span className="error">{errors.email}</span>}
        </div>

        <div className="form-group">
          <label>Date of Birth</label>
          <input
            name="dateOfBirth"
            type="date"
            value={employee.dateOfBirth}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.dateOfBirth && <span className="error">{errors.dateOfBirth}</span>}
        </div>

        <div className="form-group">
          <label>Salary</label>
          <input
            name="salary"
            type="number"
            value={employee.salary}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.salary && <span className="error">{errors.salary}</span>}
        </div>

        <div className="form-group">
          <label>Department</label>
          <select
            name="departmentId"
            value={employee.departmentId}
            onChange={handleChange}
            required
            className="input-field"
          >
            <option value="">--Select Department--</option>
            {departments.map((dept) => (
              <option key={dept.departmentId} value={dept.departmentId}>
                {dept.departmentName}
              </option>
            ))}
          </select>
          {errors.departmentId && <span className="error">{errors.departmentId}</span>}
        </div>

        <button type="submit" className="submit-btn">
          {id ? 'Update' : 'Create'}
        </button>
      </form>
    </div>
  );
}
