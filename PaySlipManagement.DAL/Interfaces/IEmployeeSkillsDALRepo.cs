using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface IEmployeeSkillsDALRepo
    {
        Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkillsAsync();
        Task<EmployeeSkills> GetEmployeeSkillsByidAsync(EmployeeSkills _employeeSkills);
        Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByEmpCodeAsync(string Emp_Code);
        Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByManagerAsync(string Emp_Code);
        Task<bool> CreateEmployeeSkills(EmployeeSkills _employeeSkills);
        Task<bool> UpdateEmployeeSkills(EmployeeSkills _employeeSkills);
        Task<bool> DeleteEmployeeSkills(EmployeeSkills _employeeSkills);
    }
}

