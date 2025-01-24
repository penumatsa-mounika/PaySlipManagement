using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Implementations
{
    public class DocumentDALRepo
    {
        DapperServices<Document> _db;
        public DocumentDALRepo()
        {
            _db = new DapperServices<Document>();
        }
        public async Task<bool> Create(Document pdf)
        {
            try
            {
                if (pdf != null)
                {
                    await _db.CreateAsync(pdf);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> GetByIdAsync(string empcode, string doc)
        {

            try
            {
                Document d = new Document();
                d.Emp_Code = empcode;
                d.DocumentType = doc;
                DapperServices<Document> document = new DapperServices<Document>();
                return await document.ReadGetByTypeAsync(d);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
