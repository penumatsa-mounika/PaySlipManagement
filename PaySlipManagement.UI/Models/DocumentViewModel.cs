namespace PaySlipManagement.UI.Models
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Emp_Code { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        //public DateTime UploadDate { get; set; }
    }
}
