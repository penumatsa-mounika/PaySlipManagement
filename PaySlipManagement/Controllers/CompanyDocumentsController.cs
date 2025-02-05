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
        //[HttpGet("GetCompanyDocumentsByIdAsync/{empcode}/{document}")]
        //public async Task<CompanyDocuments> GetByIdDocumentAsync(string empcode, string document)
        //{
        //    return await _documentBALRepo.GetByIdAsync(empcode, document);
        //}
        [HttpPost("Create")]
        public async Task<bool> Create(CompanyDocuments doc)
        {
            return await _documentBALRepo.Create(doc);
        }
    }
}