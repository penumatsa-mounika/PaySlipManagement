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
    public class EmployeeRegularizationDALRepo:IEmployeeRegularizationDALRepo
    {
        DapperServices<EmployeeRegularization> employeeRegularizationRepository;
        public EmployeeRegularizationDALRepo()
        {
            employeeRegularizationRepository = new DapperServices<EmployeeRegularization>();
        }

        public async Task<IEnumerable<EmployeeRegularization>> GetAllEmployeeRegularizationAsync()
        {
            try
            {
                var result = await employeeRegularizationRepository.ReadAllAsync(new EmployeeRegularization() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmployeeRegularization> GetEmployeeRegularizationByidAsync(EmployeeRegularization _employee)
        {
            try
            {
                return await employeeRegularizationRepository.ReadGetByIdAsync(_employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByManagerAsync(string Emp_Code)
        {
            try
            {
                EmployeeRegularization er = new EmployeeRegularization();
                er.Emp_Code = Emp_Code;
                DapperServices<EmployeeRegularization> _regularizationRepo = new DapperServices<EmployeeRegularization>();
                return await _regularizationRepo.ReadGetCodeByAllAsync(er);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByCodeAsync(string empcode, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                EmployeeRegularization task = new EmployeeRegularization();
                task.Emp_Code = empcode;
                DapperServices<EmployeeRegularization> _employeeRegularizationRepo = new DapperServices<EmployeeRegularization>();
                return await _employeeRegularizationRepo.ReadGetCodeByEmpCodeAsync(empcode, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateEmployeeRegularization(EmployeeRegularization _employee)
        {
            try
            {
                var employeeExists = await employeeRegularizationRepository.CheckEmployeeExistsAsync(_employee.Emp_Code);
                if (!employeeExists)
                {
                    return false; // Employee does not exist, creation cannot proceed
                }
                if (_employee != null)
                {
                    await employeeRegularizationRepository.CreateAsync(_employee);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateEmployeeRegularization(EmployeeRegularization _employee)
        {
            try
            {
                if (_employee != null)
                {
                    var employeeExists = await employeeRegularizationRepository.CheckEmployeeExistsAsync(_employee.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await employeeRegularizationRepository.UpdateAsync(_employee);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteEmployeeRegularization(EmployeeRegularization employee)
        {
            try
            {
                if (employee != null)
                {
                    await employeeRegularizationRepository.DeleteAsync(employee);
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
