using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentBALRepo _documentBALRepo;
        public DocumentController (IDocumentBALRepo documentBALRepo)
        {
            _documentBALRepo = documentBALRepo;
        }
        [HttpGet("GetDocumentsByIdAsync/{empcode}/{document}")]
        public async Task<Document> GetByIdDocumentAsync(string empcode,string document)
        {
            return await _documentBALRepo.GetByIdAsync(empcode,document);
        }
        [HttpPost("CreateDocumentAsync")]
        public async Task<bool> Create(Document doc)
        {
            return await _documentBALRepo.Create(doc);
        }
    }
}
