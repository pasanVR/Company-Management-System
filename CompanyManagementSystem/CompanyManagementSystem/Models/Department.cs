using System.ComponentModel.DataAnnotations;

namespace CompanyManagementSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(10, ErrorMessage = "Department code cannot exceed 10 characters.")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(200, ErrorMessage = "Department name cannot exceed 200 characters.")]
        public string DepartmentName { get; set; }
    }
}
