using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface ICompanyDocumentsBALRepo
    {
        Task<IEnumerable<CompanyDocuments>> GetByIdAsync(string empcode, string doc);
        Task<CompanyDocuments> GetCompanyDocumentsByidAsync(CompanyDocuments _doc);
        Task<bool> Create(CompanyDocuments pdf);
        Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByManagerAsync(string approvalPerson);
        Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByEmpCodeAsync(string Emp_Code);
        Task<bool> UpdateCompanyDocuments(CompanyDocuments _doc);

    }
}