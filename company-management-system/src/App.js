import React from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import DepartmentList from './components/Departments/DepartmentList';
import DepartmentForm from './components/Departments/DepartmentForm';
import EmployeeList from './components/Employees/EmployeeList';
import EmployeeForm from './components/Employees/EmployeeForm';

export default function App() {
  return (
    <div className="container">
      <h1>Company Management System</h1>
      <nav style={{ marginBottom: '20px' }}>
        <Link to="/departments" style={{ marginRight: '15px' }}>Departments</Link>
        <Link to="/employees">Employees</Link>
      </nav>

      <Routes>
        {/* Departments */}
        <Route path="/departments" element={<DepartmentList />} />
        <Route path="/departments/add" element={<DepartmentForm />} />
        <Route path="/departments/edit/:id" element={<DepartmentForm />} />

        {/* Employees */}
        <Route path="/employees" element={<EmployeeList />} />
        <Route path="/employees/add" element={<EmployeeForm />} />
        <Route path="/employees/edit/:id" element={<EmployeeForm />} />

        {/* Default Redirect */}
        <Route path="*" element={<DepartmentList />} />
      </Routes>
    </div>
  );
}
