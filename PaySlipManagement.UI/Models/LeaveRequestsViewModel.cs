using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class LeaveRequestsViewModel
    {
        public int? Id { get; set; }
        [Display(Name = "Employee Code")]
        public string? Emp_Code { get; set; }
        [Display(Name = "Leave Type")]
        public string? LeaveType { get; set; }
        public string? Reason { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        [Required(ErrorMessage = "From Date is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "To Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }
        [Display(Name = "Leaves Count")]
        public decimal? LeavesCount { get; set; }
        [Display(Name = "Approval Person")]
        public string? ApprovalPerson { get; set; }
        public string? Status { get; set; }
    }
}
