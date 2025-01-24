using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class EmployeeRegularizationViewModel
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }

        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal DeviationInWorkingHours { get; set; }
        public string? Regularization { get; set; }
        public string ApprovalPerson { get; set; }
        public string Status { get; set; }
    }
}
