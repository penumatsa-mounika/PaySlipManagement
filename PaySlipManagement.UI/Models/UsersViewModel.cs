

using System.ComponentModel.DataAnnotations;

namespace PaySlipManagement.UI.Models
{
    public class UsersViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Employee Code")]
        public string? Emp_Code { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$")]
        //,ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //public string Role { get; set; }
    }
}
