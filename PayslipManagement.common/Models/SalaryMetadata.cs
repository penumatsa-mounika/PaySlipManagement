﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayslipManagement.Common.Models
{
    public class SalaryMetadata
    {
        public string Emp_Code { get; set; }
        public DateTime PaySlipForMonth { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal DaysPaid { get; set; }
        public decimal EarnedBasic { get; set; }
        public decimal HRA { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal TDS { get; set; }
        public decimal Advance { get; set; }
        public decimal PFEmployeeShare { get; set; }
        public decimal NetPay { get; set; }
        public decimal EarningTotal { get;set; }
        public decimal TotalDeductions { get; set; }
    }
}
