using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface IDocumentDALRepo
    {
        Task<Document> GetByIdAsync(string empcode, string doc);
        Task<bool> Create(Document user);
    }
}
