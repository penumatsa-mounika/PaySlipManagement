using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDocumentsController : ControllerBase
    {
        private readonly ICompanyDocumentsBALRepo _documentBALRepo;
        public CompanyDocumentsController(ICompanyDocumentsBALRepo documentBALRepo)
        {
            _documentBALRepo = documentBALRepo;
        }
        [HttpGet("GetCompanyDocumentsByIdAsync/{empcode}/{document}")]
        public async Task<IEnumerable<CompanyDocuments>> GetByIdDocumentAsync(string empcode, string document)
        {
            return await _documentBALRepo.GetByIdAsync(empcode, document);
        }
        [HttpGet("GetCompanyDocumentsById/{id}")]
        public async Task<CompanyDocuments> GetCompanyDocumentsByidAsync(int id)
        {
            CompanyDocuments d = new CompanyDocuments();
            d.Id = id;
            return await _documentBALRepo.GetCompanyDocumentsByidAsync(d);
        }

        [HttpPost("Create")]
        public async Task<bool> Create(CompanyDocuments doc)
        {
            return await _documentBALRepo.Create(doc);
        }

        [HttpGet("GetCompanyDocumentsByManager/{Emp_Code}")]
        public async Task<IEnumerable<CompanyDocuments>> GetEmployeeRegularizationByManagerAsync(string Emp_Code)
        {
            return await _documentBALRepo.GetCompanyDocumentsByManagerAsync(Emp_Code);
        }
        [HttpGet("GetCompanyDocumentsByCode/{Emp_Code}")]
        public async Task<IEnumerable<CompanyDocuments>> GetCompanyDocumentsByEmpCodeAsync(string Emp_Code)
        {
            return await _documentBALRepo.GetCompanyDocumentsByEmpCodeAsync(Emp_Code);
        }
        [HttpPut("UpdateCompanyDocuments")]
        public async Task<bool> UpdateCompanyDocuments(CompanyDocuments _doc)
        {
            return await _documentBALRepo.UpdateCompanyDocuments(_doc);

        }
    }
}