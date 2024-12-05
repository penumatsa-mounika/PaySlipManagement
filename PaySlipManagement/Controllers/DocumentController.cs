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
        [HttpGet("GetDocumentsByIdAsync")]
        public async Task<Document> GetByIdDocumentAsync()
        {
            Document d = new Document();
            d.Id = null;
            return await _documentBALRepo.GetByIdAsync(d);
        }
        [HttpPost("CreateDocumentAsync")]
        public async Task<bool> Create(Document doc)
        {
            return await _documentBALRepo.Create(doc);
        }
    }
}
