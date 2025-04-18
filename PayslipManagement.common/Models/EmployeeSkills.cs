using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class EmployeeSkills
    {
        public int? Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? SkillName { get; set; }
        public string? ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Status { get; set; } // "Pending", "Approved", "Declined"
        public string? ApprovalPerson { get; set; }
    }
}

