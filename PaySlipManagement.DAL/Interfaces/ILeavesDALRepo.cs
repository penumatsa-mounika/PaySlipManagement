using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface ILeavesDALRepo
    {
        Task<IEnumerable<Leaves>> GetAllLeavesAsync();
        Task<Leaves> GetLeavesByidAsync(Leaves _leaves);
        Task<Leaves> GetLeavesByCodeAsync(string Emp_Code);
        Task<bool> CreateLeaves(Leaves _leaves);
        Task<bool> UpdateLeaves(Leaves _leaves);
        Task<bool> DeleteLeaves(Leaves leaves);
        Task CarryForwardLeavesAsync();
        Task MonthlyLeaveAdditionAsync();
    }
}
