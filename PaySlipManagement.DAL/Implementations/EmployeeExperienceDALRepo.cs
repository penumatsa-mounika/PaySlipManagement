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
    public class EmployeeExperienceDALRepo : IEmployeeExperienceDALRepo
    {
        DapperServices<EmployeeExperience> employeeExperienceRepository;
        public EmployeeExperienceDALRepo()
        {
            employeeExperienceRepository = new DapperServices<EmployeeExperience>();
        }

        public async Task<IEnumerable<EmployeeExperience>> GetAllEmployeeExperienceAsync()
        {
            try
            {
                var result = await employeeExperienceRepository.ReadAllAsync(new EmployeeExperience() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmployeeExperience> GetEmployeeExperienceByidAsync(EmployeeExperience _employeeExperience)
        {
            try
            {
                return await employeeExperienceRepository.ReadGetByIdAsync(_employeeExperience);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByEmpCodeAsync(string Emp_Code)
        {
            try
            {
                EmployeeExperience es = new EmployeeExperience();
                es.Emp_Code = Emp_Code;
                DapperServices<EmployeeExperience> _regularizationRepo = new DapperServices<EmployeeExperience>();
                return await _regularizationRepo.ReadGetByAllNullCodeAsync(es);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByManagerAsync(string Emp_Code)
        {
            try
            {
                EmployeeExperience es = new EmployeeExperience();
                es.Emp_Code = Emp_Code;
                DapperServices<EmployeeExperience> _regularizationRepo = new DapperServices<EmployeeExperience>();
                return await _regularizationRepo.ReadGetCodeByAllAsync(es);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            try
            {
                var employeeExists = await employeeExperienceRepository.CheckEmployeeExistsAsync(_employeeExperience.Emp_Code);
                if (!employeeExists)
                {
                    return false; // Employee does not exist, creation cannot proceed
                }
                if (_employeeExperience != null)
                {
                    await employeeExperienceRepository.CreateAsync(_employeeExperience);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            try
            {
                if (_employeeExperience != null)
                {
                    var employeeExists = await employeeExperienceRepository.CheckEmployeeExistsAsync(_employeeExperience.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await employeeExperienceRepository.UpdateAsync(_employeeExperience);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteEmployeeExperience(EmployeeExperience employeeExperience)
        {
            try
            {
                if (employeeExperience != null)
                {
                    await employeeExperienceRepository.DeleteAsync(employeeExperience);
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