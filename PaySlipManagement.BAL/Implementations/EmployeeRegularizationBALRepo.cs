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
    public class EmployeeRegularizationBALRepo:IEmployeeRegularizationBALRepo
    {
        public EmployeeRegularizationDALRepo _employeeRegularizationDALRepo = new EmployeeRegularizationDALRepo();
        public async Task<IEnumerable<EmployeeRegularization>> GetAllEmployeeRegularizationAsync()
        {
            return await _employeeRegularizationDALRepo.GetAllEmployeeRegularizationAsync();
        }

        public async Task<EmployeeRegularization> GetEmployeeRegularizationByidAsync(EmployeeRegularization _employee)
        {
            return await _employeeRegularizationDALRepo.GetEmployeeRegularizationByidAsync(_employee);
        }
        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByManagerAsync(string Emp_Code)
        {
            return await _employeeRegularizationDALRepo.GetEmployeeRegularizationByManagerAsync(Emp_Code);
        }
        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByCodeAsync(string Emp_Code, DateTime StartDate, DateTime EndDate)
        {
            return await _employeeRegularizationDALRepo.GetEmployeeRegularizationByCodeAsync(Emp_Code, StartDate, EndDate);
        }
        public async Task<bool> CreateEmployeeRegularization(EmployeeRegularization _employee)
        {
            return await _employeeRegularizationDALRepo.CreateEmployeeRegularization(_employee);

        }
        public async Task<bool> UpdateEmployeeRegularization(EmployeeRegularization _employee)
        {
            return await _employeeRegularizationDALRepo.UpdateEmployeeRegularization(_employee);

        }
        public async Task<bool> DeleteEmployeeRegularization(EmployeeRegularization employee)
        {
            return await _employeeRegularizationDALRepo.DeleteEmployeeRegularization(employee);

        }
    }
}
