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

        public async Task<CompanyDocuments> GetDepartmentByidAsync(CompanyDocuments _doc)
        {
            try
            {
                return await _db.ReadGetByIdAsync(_doc);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<CompanyDocuments>> GetByIdAsync(string empcode, string doc)
        {

            try
            {
                CompanyDocuments d = new CompanyDocuments();
                d.Emp_Code = empcode;
                d.DocumentType = doc;
                DapperServices<CompanyDocuments> document = new DapperServices<CompanyDocuments>();
                return await document.ReadGetAllByTypeAsync(d);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByManagerAsync(string Emp_Code)
        {
            try
            {
                CompanyDocuments er = new CompanyDocuments();
                er.Emp_Code = Emp_Code;
                DapperServices<CompanyDocuments> document = new DapperServices<CompanyDocuments>();
                return await document.ReadGetCodeByAllAsync(er);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByEmpCodeAsync(string Emp_Code)
        {
            try
            {
                CompanyDocuments er = new CompanyDocuments();
                er.Emp_Code = Emp_Code;
                DapperServices<CompanyDocuments> _documentRepo = new DapperServices<CompanyDocuments>();
                return await _documentRepo.ReadGetByAllNullCodeAsync(er);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateCompanyDocuments(CompanyDocuments _doc)
        {
            try
            {
                if (_doc != null)
                {
                    var employeeExists = await _db.CheckEmployeeExistsAsync(_doc.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await _db.UpdateAsync(_doc);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}