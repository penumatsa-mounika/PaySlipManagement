

using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class UsersViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Employee Code")]
        public string? Emp_Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //public string Role { get; set; }
    }
}
