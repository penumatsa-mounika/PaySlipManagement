using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class ManagerViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Employee Code")]
        public string Emp_Code { get; set; }

        [Display(Name = "Manager Code")]
        public string ManagerCode { get; set; }
    }
}
