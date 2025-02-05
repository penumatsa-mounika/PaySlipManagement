using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Implementations
{
    public class CompanyDocumentsBALRepo : ICompanyDocumentsBALRepo
    {
        private CompanyDocumentsDALRepo _docDALRepo;

        public CompanyDocumentsBALRepo()
        {
            _docDALRepo = new CompanyDocumentsDALRepo();
        }
        public async Task<bool> Create(CompanyDocuments pdf)
        {
            return await _docDALRepo.Create(pdf);
        }

        public async Task<CompanyDocuments> GetByIdAsync(string empcode, string doc)
        {
            return await _docDALRepo.GetByIdAsync(empcode, doc);
        }
    }
}