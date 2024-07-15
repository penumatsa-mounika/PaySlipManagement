using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayslipManagement.Common.Models
{
    public class EmployeeDetails
    {
        // Employee Information
        public string Emp_Code { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string Email { get; set; }
        public string PAN_Number { get; set; }
        // Nullable DateTime to handle null values
        private DateTime? _joiningDate;
        public DateTime? JoiningDate
        {
            get { return _joiningDate; }
            set
            {
                // Ensure valid DateTime value
                if (value.HasValue && value.Value < new DateTime(1753, 1, 1))
                    throw new ArgumentOutOfRangeException("JoiningDate", "Date must be on or after 01/01/1753");
                _joiningDate = value;
            }
        }
        // Salary Metadata
        public string PaySlipForMonth { get; set; }
        public decimal DaysPaid { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal EarnedBasic { get; set; }
        public decimal HRA { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal PFEmployeeShare { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal TDS { get; set; }
        public decimal EarningTotal { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal AnnualCTC { get; set; }
        public decimal CTCMonth { get; set; }
        public decimal PFEmployerShare { get; set; }
        public decimal PFEmployerShareAnnual { get; set; }
        public decimal AnnualGrossPay { get; set; }
        public decimal MonthGrossPay { get; set; }
        public decimal OtherAdditions { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal LossOfPay { get; set; }
        // Account Details
        public string BankName { get; set; }
        public long BankAccountNumber { get; set; }
        public long UANNumber { get; set; }
        public string PFAccountNumber { get; set; }
        //company details
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
    }
}
