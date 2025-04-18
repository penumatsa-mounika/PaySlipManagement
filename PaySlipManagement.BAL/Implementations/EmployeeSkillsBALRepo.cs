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
    public class EmployeeSkillsBALRepo : IEmployeeSkillsBALRepo
    {
        public EmployeeSkillsDALRepo _employeeSkillsDALRepo = new EmployeeSkillsDALRepo();
        public async Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkillsAsync()
        {
            return await _employeeSkillsDALRepo.GetAllEmployeeSkillsAsync();
        }

        public async Task<EmployeeSkills> GetEmployeeSkillsByidAsync(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsDALRepo.GetEmployeeSkillsByidAsync(_employeeSkills);
        }
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByEmpCodeAsync(string Emp_Code)
        {
            return await _employeeSkillsDALRepo.GetEmployeeSkillsByEmpCodeAsync(Emp_Code);
        }
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByManagerAsync(string Emp_Code)
        {
            return await _employeeSkillsDALRepo.GetEmployeeSkillsByManagerAsync(Emp_Code);
        }
        public async Task<bool> CreateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsDALRepo.CreateEmployeeSkills(_employeeSkills);

        }
        public async Task<bool> UpdateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsDALRepo.UpdateEmployeeSkills(_employeeSkills);

        }
        public async Task<bool> DeleteEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsDALRepo.DeleteEmployeeSkills(_employeeSkills);

        }
    }
}

