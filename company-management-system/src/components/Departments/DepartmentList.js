import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../../services/api';

export default function DepartmentList() {
  const [departments, setDepartments] = useState([]);

  useEffect(() => {
    fetchDepartments();
  }, []);

  const fetchDepartments = async () => {
    try {
      const response = await api.get('/department');
      setDepartments(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  const deleteDepartment = async (id) => {
    if (!window.confirm('Are you sure you want to delete this department?')) return;
    try {
      await api.delete(`/department/${id}`);
      fetchDepartments();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <h2>Departments</h2>
      <Link to="/departments/add"><button>Add Department</button></Link>
      <table border="1" cellPadding="8" style={{ marginTop: '10px', width: '100%' }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Code</th>
            <th>Name</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {departments.map((dept) => (
            <tr key={dept.departmentId}>
              <td>{dept.departmentId}</td>
              <td>{dept.departmentCode}</td>
              <td>{dept.departmentName}</td>
              <td>
                <Link to={`/departments/edit/${dept.departmentId}`}><button>Edit</button></Link>
                <button onClick={() => deleteDepartment(dept.departmentId)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
