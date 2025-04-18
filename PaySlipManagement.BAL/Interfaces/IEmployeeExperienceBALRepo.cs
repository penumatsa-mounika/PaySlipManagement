using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface IEmployeeExperienceBALRepo
    {
        Task<IEnumerable<EmployeeExperience>> GetAllEmployeeExperienceAsync();
        Task<EmployeeExperience> GetEmployeeExperienceByidAsync(EmployeeExperience _employeeExperience);
        Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByEmpCodeAsync(string Emp_Code);
        Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByManagerAsync(string Emp_Code);
        Task<bool> CreateEmployeeExperience(EmployeeExperience _employeeExperience);
        Task<bool> UpdateEmployeeExperience(EmployeeExperience _employeeExperience);
        Task<bool> DeleteEmployeeExperience(EmployeeExperience _employeeExperience);
    }
}
