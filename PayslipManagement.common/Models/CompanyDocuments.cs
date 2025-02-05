using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class CompanyDocuments
    {
        public int Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? DocumentType { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
    }
}