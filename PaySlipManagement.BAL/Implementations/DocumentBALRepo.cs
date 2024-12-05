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
    public class DocumentBALRepo: IDocumentBALRepo
    {
        private DocumentDALRepo _documentDALRepo;

        public DocumentBALRepo()
        {
            _documentDALRepo = new DocumentDALRepo();
        }
        public async Task<bool> Create(Document pdf)
        {
            return await _documentDALRepo.Create(pdf);
        }

        public async Task<Document> GetByIdAsync(Document pdf)
        {
            return await _documentDALRepo.GetByIdAsync(pdf);
        }
    }
}
