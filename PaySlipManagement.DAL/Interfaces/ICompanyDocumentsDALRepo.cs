using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface ICompanyDocumentsDALRepo
    {
        Task<IEnumerable<CompanyDocuments>> GetByIdAsync(string empcode, string doc);
        Task<CompanyDocuments> GetCompanyDocumentsByidAsync(CompanyDocuments _department);
        Task<bool> Create(CompanyDocuments user);
        Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByManagerAsync(string approvalPerson);
        Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByEmpCodeAsync(string Emp_Code);
        Task<bool> UpdateCompanyDocuments(CompanyDocuments _doc);

    }
}