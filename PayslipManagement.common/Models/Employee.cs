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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Designation { get; set; }
        public String Division { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public String Email { get; set; }
        public string PAN_Number { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }
        public bool IsActive { get; set; } = true;
        public long PhoneNumber { get; set; }
        public long EmergencyContact { get; set; }
        public string EmergencyRelation { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PersonalEmail { get; set; }
        public string BloodGroup { get; set; }
        public long AadharNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfCelebration { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string ChildrenNames { get; set; }
        public string LinkedInProfile {  get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public long BankAccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string PreviousCompanyName { get; set; }
        public string Tenure { get; set; }
        public string ReportingManager { get; set; }
        public string LatestCompanyReference { get; set; }
    }
}
