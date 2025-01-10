using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class EmployeeRegularization
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
    }
}
