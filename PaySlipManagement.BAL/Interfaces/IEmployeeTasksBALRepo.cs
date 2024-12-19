using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface IEmployeeTasksBALRepo
    {
        Task<IEnumerable<EmployeeTasks>> GetAllEmployeeTasksAsync();

        Task<EmployeeTasks> GetEmployeeTasksByidAsync(EmployeeTasks _employeeTasks);
        Task<IEnumerable<EmployeeTasks>> GetEmployeeTasksByCodeAsync(string Emp_Code);
        Task<bool> CreateEmployeeTasks(EmployeeTasks _employeeTasks);
        Task<bool> UpdateEmployeeTasks(EmployeeTasks _employeeTasks);
        Task<bool> DeleteEmployeeTasks(EmployeeTasks employeeTasks);
    }
}
