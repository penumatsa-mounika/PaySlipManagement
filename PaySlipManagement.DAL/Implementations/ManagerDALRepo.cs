using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Implementations
{
    public class ManagerDALRepo: IManagerDALRepo
    {
        DapperServices<Manager> ManagerRepository;
        public ManagerDALRepo()
        {
            ManagerRepository = new DapperServices<Manager>();
        }


        public async Task<IEnumerable<Manager>> GetAllManagerAsync()
        {
            try
            {
                var result = await ManagerRepository.ReadAllAsync(new Manager() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Manager> GetManagerByidAsync(Manager _manager)
        {
            try
            {
                return await ManagerRepository.ReadGetByIdAsync(_manager);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> CreateManager(Manager _manager)
        {
            try
            {

                if (_manager != null)
                {
                    await ManagerRepository.CreateAsync(_manager);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateManager(Manager _manager)
        {
            try
            {
                if (_manager != null)
                {
                    await ManagerRepository.UpdateAsync(_manager);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> DeleteManager(Manager manager)
        {
            try
            {
                if (manager != null)
                {
                    await ManagerRepository.DeleteAsync(manager);
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
