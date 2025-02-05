using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Implementations
{
    public class CompanyDocumentsDALRepo
    {
        DapperServices<CompanyDocuments> _db;
        public CompanyDocumentsDALRepo()
        {
            _db = new DapperServices<CompanyDocuments>();
        }
        public async Task<bool> Create(CompanyDocuments pdf)
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

        public async Task<CompanyDocuments> GetByIdAsync(string empcode, string doc)
        {

            try
            {
                CompanyDocuments d = new CompanyDocuments();
                d.Emp_Code = empcode;
                d.DocumentType = doc;
                DapperServices<CompanyDocuments> document = new DapperServices<CompanyDocuments>();
                return await document.ReadGetByTypeAsync(d);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}