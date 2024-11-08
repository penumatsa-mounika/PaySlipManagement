using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class Leaves
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }
        public int TypeId { get; set; }
        public decimal TotalLeaves { get; set; }
        public decimal LeavesAvailable { get; set; }
        public decimal LeavesUsed { get; set; }
        public int Year { get; set; }
        public decimal? CarriedForwardLeaves { get; set; }
    }
}
