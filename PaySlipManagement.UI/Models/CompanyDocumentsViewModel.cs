namespace PaySlipManagement.UI.Models
{
    public class CompanyDocumentsViewModel
    {
        public int? Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? DocumentType { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
    }
}