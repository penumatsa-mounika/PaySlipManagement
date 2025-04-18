using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeExperienceController : ControllerBase
    {
        private readonly IEmployeeExperienceBALRepo _employeeExperienceBALRepo;
        public EmployeeExperienceController(IEmployeeExperienceBALRepo employeeExperienceBALRepo)
        {
            _employeeExperienceBALRepo = employeeExperienceBALRepo;
        }
        [HttpGet("GetAllEmployeeExperience")]
        public async Task<IEnumerable<EmployeeExperience>> GetAllEmployeeExperienceAsync()
        {
            return await _employeeExperienceBALRepo.GetAllEmployeeExperienceAsync();
        }
        [HttpGet("GetEmployeeExperienceByid/{id}")]
        public async Task<EmployeeExperience> GetEmployeeExperienceByidAsync(int id)
        {
            EmployeeExperience _employeeExperience = new EmployeeExperience();
            _employeeExperience.Id = id;
            return await _employeeExperienceBALRepo.GetEmployeeExperienceByidAsync(_employeeExperience);
        }
        [HttpGet("GetEmployeeExperienceByCode/{Emp_Code}")]
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByEmpCodeAsync(string Emp_Code)
        {
            return await _employeeExperienceBALRepo.GetEmployeeExperienceByEmpCodeAsync(Emp_Code);
        }
        [HttpGet("GetEmployeeExperienceByManager/{Emp_Code}")]
        public async Task<IEnumerable<EmployeeExperience>> GetEmployeeExperienceByManagerAsync(string Emp_Code)
        {
            return await _employeeExperienceBALRepo.GetEmployeeExperienceByManagerAsync(Emp_Code);
        }

        [HttpPost("CreateEmployeeExperience")]
        public async Task<bool> CreateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceBALRepo.CreateEmployeeExperience(_employeeExperience);

        }
        [HttpPut("UpdateEmployeeExperience")]
        public async Task<bool> UpdateEmployeeExperience(EmployeeExperience _employeeExperience)
        {
            return await _employeeExperienceBALRepo.UpdateEmployeeExperience(_employeeExperience);

        }
        [HttpGet("DeleteEmployeeExperience/{id}")]
        public async Task<bool> DeleteEmployeeExperience(int id)
        {
            EmployeeExperience employeeExperience = new EmployeeExperience();
            employeeExperience.Id = id;
            return await _employeeExperienceBALRepo.DeleteEmployeeExperience(employeeExperience);
        }
    }
}
