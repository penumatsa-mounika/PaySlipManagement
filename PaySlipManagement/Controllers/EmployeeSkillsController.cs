using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSkillsController : ControllerBase
    {
        private readonly IEmployeeSkillsBALRepo _employeeSkillsBALRepo;
        public EmployeeSkillsController(IEmployeeSkillsBALRepo employeeSkillsBALRepo)
        {
            _employeeSkillsBALRepo = employeeSkillsBALRepo;
        }
        [HttpGet("GetAllEmployeeSkills")]
        public async Task<IEnumerable<EmployeeSkills>> GetAllEmployeeSkillsAsync()
        {
            return await _employeeSkillsBALRepo.GetAllEmployeeSkillsAsync();
        }
        [HttpGet("GetEmployeeSkillsByid/{id}")]
        public async Task<EmployeeSkills> GetEmployeeSkillsByidAsync(int id)
        {
            EmployeeSkills _employeeSkills = new EmployeeSkills();
            _employeeSkills.Id = id;
            return await _employeeSkillsBALRepo.GetEmployeeSkillsByidAsync(_employeeSkills);
        }
        [HttpGet("GetEmployeeSkillsByCode/{Emp_Code}")]
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByEmpCodeAsync(string Emp_Code)
        {
            return await _employeeSkillsBALRepo.GetEmployeeSkillsByEmpCodeAsync(Emp_Code);
        }
        [HttpGet("GetEmployeeSkillsByManager/{Emp_Code}")]
        public async Task<IEnumerable<EmployeeSkills>> GetEmployeeSkillsByManagerAsync(string Emp_Code)
        {
            return await _employeeSkillsBALRepo.GetEmployeeSkillsByManagerAsync(Emp_Code);
        }

        [HttpPost("CreateEmployeeSkills")]
        public async Task<bool> CreateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsBALRepo.CreateEmployeeSkills(_employeeSkills);

        }
        [HttpPut("UpdateEmployeeSkills")]
        public async Task<bool> UpdateEmployeeSkills(EmployeeSkills _employeeSkills)
        {
            return await _employeeSkillsBALRepo.UpdateEmployeeSkills(_employeeSkills);

        }
        [HttpGet("DeleteEmployeeSkills/{id}")]
        public async Task<bool> DeleteEmployeeSkills(int id)
        {
            EmployeeSkills employeeSkills = new EmployeeSkills();
            employeeSkills.Id = id;
            return await _employeeSkillsBALRepo.DeleteEmployeeSkills(employeeSkills);
        }
    }
}

