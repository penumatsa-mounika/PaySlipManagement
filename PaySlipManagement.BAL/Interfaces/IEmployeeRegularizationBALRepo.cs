using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface IEmployeeRegularizationBALRepo
    {
        Task<IEnumerable<EmployeeRegularization>> GetAllEmployeeRegularizationAsync();

        Task<EmployeeRegularization> GetEmployeeRegularizationByidAsync(EmployeeRegularization _employee);
        Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByManagerAsync(string Emp_Code);
        Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByCodeAsync(string Emp_Code, DateTime StartDate, DateTime EndDate);
        Task<bool> CreateEmployeeRegularization(EmployeeRegularization _employee);
        Task<bool> UpdateEmployeeRegularization(EmployeeRegularization _employee);
        Task<bool> DeleteEmployeeRegularization(EmployeeRegularization employee);
    }
}
