using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Designation { get; set; }
        public String Division { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public String Email { get; set; }
        public string PAN_Number { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }
        public bool IsActive { get; set; } = true;
        public long PhoneNumber { get; set; }

    }
}
