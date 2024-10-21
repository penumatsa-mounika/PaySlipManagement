using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface IManagerDALRepo
    {
        Task<IEnumerable<Manager>> GetAllManagerAsync();
        Task<Manager> GetManagerByidAsync(Manager _manager);
        Task<bool> CreateManager(Manager _manager);
        Task<bool> UpdateManager(Manager _manager);
        Task<bool> DeleteManager(Manager manager);
    }
}
