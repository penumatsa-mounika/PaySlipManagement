namespace PaySlipManagement.UI.Models
{
    public class EmployeeExperienceViewModel
    {
        public int? Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? Role { get; set; }
        public string? CompanyName { get; set; }
        public string? Responsibility { get; set; }
        public int? StartMonth { get; set; }
        public int? StartYear { get; set; }
        public int? EndMonth { get; set; }
        public int? EndYear { get; set; }
        public string? ApprovalPerson { get; set; }
        public string? Status { get; set; }
    }
}
