using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Interfaces
{
    public interface IEmployeeDALRepo
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(Employee _employee);
        Task<IEnumerable<EmployeeDetails>> GetAllEmployeesDetailsAsync();
        Task<EmployeeDetails> GetEmployeeByCodeAsync(string empcode, string payperiod);
        Task<EmployeeDetails> GetEmployeeDetailsByCodeAsync(string empcode);
        Task<EmployeeDetails> GetEmployeeByCodeAsync(string empcode);
        Task<bool> AddEmployee(Employee _employee);
        Task<bool> UpdateEmployee(Employee _employee);
        Task<bool> DeleteEmployee(Employee _employee);

    }
}
