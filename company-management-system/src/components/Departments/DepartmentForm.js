import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../services/api';
import '../../styles/departments.css'; 

export default function DepartmentForm() {
  const [department, setDepartment] = useState({ departmentCode: '', departmentName: '' });
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    if (id) fetchDepartment(id);
  }, [id]);

  const fetchDepartment = async (id) => {
    try {
      const response = await api.get(`/department/${id}`);
      setDepartment(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setDepartment({ ...department, [name]: value });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!department.departmentCode) newErrors.departmentCode = 'Code is required';
    if (!department.departmentName) newErrors.departmentName = 'Name is required';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;
  
    try {
      if (!id) {
        // Only check for duplicate code when creating a new department
        const response = await api.get('/department');
        const existingDepartment = response.data.find(
          (dept) => dept.departmentCode === department.departmentCode
        );
  
        if (existingDepartment) {
          alert('A department with this code already exists!');
          return;
        }
  
        // Create new department
        await api.post('/department', department);
        alert('Department created!');
      } else {
        // Update existing department
        await api.put(`/department/${id}`, department);
        alert('Department updated!');
      }
  
      navigate('/departments');
    } catch (error) {
      console.error(error);
      alert('An error occurred. Please try again.');
    }
  };
  

  return (
    <div className="form-container">
      <h2>{id ? 'Edit Department' : 'Add Department'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Code</label>
          <input
            name="departmentCode"
            value={department.departmentCode}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.departmentCode && <p className="error-text">{errors.departmentCode}</p>}
        </div>
        <div className="form-group">
          <label>Name</label>
          <input
            name="departmentName"
            value={department.departmentName}
            onChange={handleChange}
            required
            className="input-field"
          />
          {errors.departmentName && <p className="error-text">{errors.departmentName}</p>}
        </div>
        <button type="submit" className="submit-btn">
          {id ? 'Update' : 'Create'}
        </button>
      </form>
    </div>
  );
}
