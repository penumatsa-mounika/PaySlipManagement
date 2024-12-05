using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.Common.Models
{
    public class Document
    {
        public int? Id { get; set; }
        public string Emp_Code { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        //public DateTime UploadDate { get; set; }
    }
}


