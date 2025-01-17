using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRegularizationController : ControllerBase
    {
        private readonly IEmployeeRegularizationBALRepo _employeeRegularizationBALRepo;
        public EmployeeRegularizationController(IEmployeeRegularizationBALRepo employeeRegBALRepo)
        {
            _employeeRegularizationBALRepo = employeeRegBALRepo;
        }
        [HttpGet("GetAllEmployeeRegularization")]
        public async Task<IEnumerable<EmployeeRegularization>> GetAllEmployeeRegularizationAsync()
        {
            return await _employeeRegularizationBALRepo.GetAllEmployeeRegularizationAsync();
        }
        [HttpGet("GetEmployeeRegularizationByid/{id}")]
        public async Task<EmployeeRegularization> GetEmployeeRegularizationByidAsync(int id)
        {
            EmployeeRegularization _employeeRegularization = new EmployeeRegularization();
            _employeeRegularization.Id = id;
            return await _employeeRegularizationBALRepo.GetEmployeeRegularizationByidAsync(_employeeRegularization);
        }
        [HttpGet("GetEmployeeRegularizationByManager/{Emp_Code}")]
        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByManagerAsync(string Emp_Code)
        {
            return await _employeeRegularizationBALRepo.GetEmployeeRegularizationByManagerAsync(Emp_Code);
        }
        [HttpGet("GetEmployeeRegularizationByEmpCode/{Emp_Code}/{StartDate}/{EndDate}")]
        public async Task<IEnumerable<EmployeeRegularization>> GetEmployeeRegularizationByCodeAsync(string Emp_Code, DateTime StartDate, DateTime EndDate)
        {
            return await _employeeRegularizationBALRepo.GetEmployeeRegularizationByCodeAsync(Emp_Code, StartDate, EndDate);
        }
        [HttpPost("CreateEmployeeRegularization")]
        public async Task<bool> CreateEmployeeRegularization(EmployeeRegularization _employeeRegularization)
        {
            return await _employeeRegularizationBALRepo.CreateEmployeeRegularization(_employeeRegularization);

        }
        [HttpPut("UpdateEmployeeRegularization")]
        public async Task<bool> UpdateEmployeeRegularization(EmployeeRegularization _employeeRegularization)
        {
            return await _employeeRegularizationBALRepo.UpdateEmployeeRegularization(_employeeRegularization);

        }
        [HttpGet("DeleteEmployeeRegularization/{id}")]
        public async Task<bool> DeleteEmployeeRegularization(int id)
        {
            EmployeeRegularization employeeRegularization = new EmployeeRegularization();
            employeeRegularization.Id = id;
            return await _employeeRegularizationBALRepo.DeleteEmployeeRegularization(employeeRegularization);
        }
    }
}
