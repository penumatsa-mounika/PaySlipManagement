using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Implementations
{
    public class ManagerBALRepo: IManagerBALRepo
    {
        public ManagerDALRepo _managerDALRepo = new ManagerDALRepo();

        public async Task<IEnumerable<Manager>> GetAllManagerAsync()
        {
            return await _managerDALRepo.GetAllManagerAsync();
        }

        public async Task<Manager> GetManagerByidAsync(Manager _manager)
        {
            return await _managerDALRepo.GetManagerByidAsync(_manager);
        }
        public async Task<bool> CreateManager(Manager _manager)
        {
            return await _managerDALRepo.CreateManager(_manager);

        }
        public async Task<bool> UpdateManager(Manager _manager)
        {
            return await _managerDALRepo.UpdateManager(_manager);

        }
        public async Task<bool> DeleteManager(Manager manager)
        {
            return await _managerDALRepo.DeleteManager(manager);

        }
    }
}
