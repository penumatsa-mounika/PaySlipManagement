using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class EmployeeTasks
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }

        [DataType(DataType.Date)]
        public DateTime TaskDate { get; set; }= DateTime.Now;
        public TimeSpan TaskFrom { get; set; }
        public TimeSpan TaskTo { get; set; } 
        public string TaskDescription { get; set; }
        public int Duration { get; set; }
        public string? Status { get; set; }
    }

}