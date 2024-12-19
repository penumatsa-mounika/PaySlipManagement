using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Implementations
{
    public class EmployeeTasksBALRepo:IEmployeeTasksBALRepo
    {
        public EmployeeTasksDALRepo _employeeTasksDALRepo = new EmployeeTasksDALRepo();
        public async Task<IEnumerable<EmployeeTasks>> GetAllEmployeeTasksAsync()
        {
            return await _employeeTasksDALRepo.GetAllEmployeeTasksAsync();
        }

        public async Task<EmployeeTasks> GetEmployeeTasksByidAsync(EmployeeTasks _leaveRequests)
        {
            return await _employeeTasksDALRepo.GetEmployeeTasksByidAsync(_leaveRequests);
        }
        public async Task<IEnumerable<EmployeeTasks>> GetEmployeeTasksByCodeAsync(string Emp_Code)
        {
            return await _employeeTasksDALRepo.GetEmployeeTasksByCodeAsync(Emp_Code);
        }
        public async Task<bool> CreateEmployeeTasks(EmployeeTasks _leaveRequests)
        {
            return await _employeeTasksDALRepo.CreateEmployeeTasks(_leaveRequests);

        }
        public async Task<bool> UpdateEmployeeTasks(EmployeeTasks _leaveRequests)
        {
            return await _employeeTasksDALRepo.UpdateEmployeeTasks(_leaveRequests);

        }
        public async Task<bool> DeleteEmployeeTasks(EmployeeTasks leaveRequests)
        {
            return await _employeeTasksDALRepo.DeleteEmployeeTasks(leaveRequests);

        }
    }
}
