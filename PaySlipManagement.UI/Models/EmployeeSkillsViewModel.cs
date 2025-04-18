namespace PaySlipManagement.UI.Models
{
    public class EmployeeSkillsViewModel
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
