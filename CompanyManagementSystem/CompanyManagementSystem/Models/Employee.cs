using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        public int DepartmentId { get; set; }
    }
}
