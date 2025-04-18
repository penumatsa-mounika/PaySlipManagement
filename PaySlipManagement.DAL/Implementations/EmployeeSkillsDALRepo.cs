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
    public class EmployeeSkillsDALRepo : IEmployeeSkillsDALRepo
    {
        DapperServices<EmployeeSkills> employeeSkillsRepository;
        public EmployeeSkillsDALRepo()
        {
            employeeSkillsRepository = new DapperServices<EmployeeSkills>();
        }

        public async Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkillsAsync()
        {
            try
            {
                var result = await employeeSkillsRepository.ReadAllAsync(new EmployeeSkills() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmployeeSkills> GetEmployeeSkillsByidAsync(EmployeeSkills _employeeSkills)
        {
            try
            {
                return await employeeSkillsRepository.ReadGetByIdAsync(_employeeSkills);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByEmpCodeAsync(string Emp_Code)
        {
            try
            {
                EmployeeSkills es = new EmployeeSkills();
                es.Emp_Code = Emp_Code;
                DapperServices<EmployeeSkills> _regularizationRepo = new DapperServices<EmployeeSkills>();
                return await _regularizationRepo.ReadGetByAllNullCodeAsync(es);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByManagerAsync(string Emp_Code)
        {
            try
            {
                EmployeeSkills es = new EmployeeSkills();
                es.Emp_Code = Emp_Code;
                DapperServices<EmployeeSkills> _regularizationRepo = new DapperServices<EmployeeSkills>();
                return await _regularizationRepo.ReadGetCodeByAllAsync(es);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            try
            {
                var employeeExists = await employeeSkillsRepository.CheckEmployeeExistsAsync(_employeeSkills.Emp_Code);
                if (!employeeExists)
                {
                    return false; // Employee does not exist, creation cannot proceed
                }
                if (_employeeSkills != null)
                {
                    await employeeSkillsRepository.CreateAsync(_employeeSkills);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            try
            {
                if (_employeeSkills != null)
                {
                    var employeeExists = await employeeSkillsRepository.CheckEmployeeExistsAsync(_employeeSkills.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await employeeSkillsRepository.UpdateAsync(_employeeSkills);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteEmployeeSkills(EmployeeSkills employeeSkills)
        {
            try
            {
                if (employeeSkills != null)
                {
                    await employeeSkillsRepository.DeleteAsync(employeeSkills);
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
