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
    public class EmployeeExperienceBALRepo : IEmployeeExperienceBALRepo
    {
        public EmployeeExperienceDALRepo _employeeExperienceDALRepo = new EmployeeExperienceDALRepo();
        public async Task<IEnumerable<EmployeeExperience>> GetAllEmployeeExperienceAsync()
        {
            return await _employeeExperienceDALRepo.GetAllEmployeeExperienceAsync();
        }

        public async Task<EmployeeExperience> GetEmployeeExperienceByidAsync(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceDALRepo.GetEmployeeExperienceByidAsync(_employeeExperience);
        }
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByEmpCodeAsync(string Emp_Code)
        {
            return await _employeeExperienceDALRepo.GetEmployeeExperienceByEmpCodeAsync(Emp_Code);
        }
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByManagerAsync(string Emp_Code)
        {
            return await _employeeExperienceDALRepo.GetEmployeeExperienceByManagerAsync(Emp_Code);
        }
        public async Task<bool> CreateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceDALRepo.CreateEmployeeExperience(_employeeExperience);

        }
        public async Task<bool> UpdateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceDALRepo.UpdateEmployeeExperience(_employeeExperience);

        }
        public async Task<bool> DeleteEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceDALRepo.DeleteEmployeeExperience(_employeeExperience);

        }
    }
}
