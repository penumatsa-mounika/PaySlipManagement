using PaySlipManagement.Common.Models;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySlipManagement.DAL.Implementations;
using PayslipManagement.Common.Models;

namespace PaySlipManagement.BAL.Implementations
{
    public class EmployeeBALRepo : IEmployeeBALRepo
    {
     //   IEmployeeDALRepo _employeeDALRepo;
        public EmployeeDALRepo _employeeDALRepo = new EmployeeDALRepo();

        //public EmployeeBALRepo(IEmployeeDALRepo employeeDALRepo)
        //{
        //    _employeeDALRepo = employeeDALRepo;
        //}
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeDALRepo.GetAllEmployees();
        }

        public async Task<Employee> GetEmployeeById(Employee _employee)
        {
            return await _employeeDALRepo.GetEmployeeById(_employee);
        }
        public async Task<IEnumerable<EmployeeDetails>> GetAllEmployeesDetailsAsync()
        {
            return await _employeeDALRepo.GetAllEmployeesDetailsAsync();
        }
        public async Task<EmployeeDetails> GetEmployeeByCodeAsync(string empcode, string payperiod)
        {
            return await _employeeDALRepo.GetEmployeeByCodeAsync(empcode,payperiod);
        }
        public async Task<EmployeeDetails> GetEmployeeByCodeAsync(string empcode)
        {
            return await _employeeDALRepo.GetEmployeeByCodeAsync(empcode);
        }
        public async Task<EmployeeDetails> GetEmployeeDetailsByCodeAsync(string empcode)
        {
            return await _employeeDALRepo.GetEmployeeDetailsByCodeAsync(empcode);
        }
        //public async Task<bool> CreateEmployee(Employee _employee)
        //{
        //    return await _employeeDALRepo.CreateEmployee(_employee);

        //}
        public async Task<bool> BulkInsertEmployees(List<Employee> employees)
        {
            try
            {
                if (employees != null && employees.Count > 0)
                {
                    await _employeeDALRepo.BulkInsertEmployees(employees);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Log to console
                throw new Exception("Error inserting bulk employees: " + ex.Message, ex);
            }

        }

        public async Task<bool> AddEmployee(Employee _employee)
        {
            return await _employeeDALRepo.AddEmployee(_employee);

        }
        public async Task<bool> UpdateEmployee(Employee _employee)
        {
            return await _employeeDALRepo.UpdateEmployee(_employee);

        }
        public async Task<bool> DeleteEmployee(Employee employee)
        {
            return await _employeeDALRepo.DeleteEmployee(employee);

        }
    }
}
