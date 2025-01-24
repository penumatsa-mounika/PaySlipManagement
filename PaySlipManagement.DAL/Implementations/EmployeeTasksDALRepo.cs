using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.DAL.Implementations
{
    public class EmployeeTasksDALRepo: IEmployeeTasksDALRepo
    {
        DapperServices<EmployeeTasks> employeeTasksRepository;
        public EmployeeTasksDALRepo()
        {
            employeeTasksRepository = new DapperServices<EmployeeTasks>();
        }

        public async Task<IEnumerable<EmployeeTasks>> GetAllEmployeeTasksAsync()
        {
            try
            {
                var result = await employeeTasksRepository.ReadAllAsync(new EmployeeTasks() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmployeeTasks> GetEmployeeTasksByidAsync(EmployeeTasks _employeeTasks)
        {
            try
            {
                return await employeeTasksRepository.ReadGetByIdAsync(_employeeTasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<EmployeeTasks>> GetEmployeeTasksByCodeAsync(string empcode, string durationfilter)
        {
            try
            {
                EmployeeTasks task = new EmployeeTasks();
                task.Emp_Code = empcode;
                DapperServices<EmployeeTasks> _employeeTasksRepo = new DapperServices<EmployeeTasks>();
                return await _employeeTasksRepo.ReadGetCodeByDurationAsync(empcode, durationfilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateEmployeeTasks(EmployeeTasks _employeeTasks)
        {
            try
            {
                var employeeExists = await employeeTasksRepository.CheckEmployeeExistsAsync(_employeeTasks.Emp_Code);
                if (!employeeExists)
                {
                    return false; // Employee does not exist, creation cannot proceed
                }
                if (_employeeTasks != null)
                {
                    await employeeTasksRepository.CreateAsync(_employeeTasks);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateEmployeeTasks(EmployeeTasks _employeeTasks)
        {
            try
            {
                if (_employeeTasks != null)
                {
                    var employeeExists = await employeeTasksRepository.CheckEmployeeExistsAsync(_employeeTasks.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await employeeTasksRepository.UpdateAsync(_employeeTasks);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteEmployeeTasks(EmployeeTasks employeeTasks)
        {
            try
            {
                if (employeeTasks != null)
                {
                    await employeeTasksRepository.DeleteAsync(employeeTasks);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
