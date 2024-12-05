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

        public async Task<Document> GetByIdAsync(Document pdf)
        {

            try
            {
                return await _db.ReadGetByIdAsync(pdf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
