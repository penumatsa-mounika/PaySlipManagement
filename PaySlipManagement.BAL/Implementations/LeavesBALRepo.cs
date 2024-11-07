using PayslipManagement.Common.Models;
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
    public class LeavesBALRepo:ILeavesBALRepo
    {
        public LeavesDALRepo _leavesDALRepo = new LeavesDALRepo();

        public async Task<IEnumerable<Leaves>> GetAllLeavesAsync()
        {
            return await _leavesDALRepo.GetAllLeavesAsync();
        }

        public async Task<Leaves> GetLeavesByidAsync(Leaves _leaves)
        {
            return await _leavesDALRepo.GetLeavesByidAsync(_leaves);
        }
        public async Task<Leaves> GetLeavesByCodeAsync(string Emp_Code)
        {
            return await _leavesDALRepo.GetLeavesByCodeAsync(Emp_Code);
        }
        public async Task<bool> CreateLeaves(Leaves _leaves)
        {
            return await _leavesDALRepo.CreateLeaves(_leaves);

        }
        public async Task<bool> UpdateLeaves(Leaves _leaves)
        {
            return await _leavesDALRepo.UpdateLeaves(_leaves);

        }
        public async Task<bool> DeleteLeaves(Leaves leaves)
        {
            return await _leavesDALRepo.DeleteLeaves(leaves);

        }

        public async Task CarryForwardLeavesAsync()
        {
            await _leavesDALRepo.CarryForwardLeavesAsync();
        }

        public async Task MonthlyLeaveAdditionAsync()
        {
            await _leavesDALRepo.MonthlyLeaveAdditionAsync();
        }
    }
}
