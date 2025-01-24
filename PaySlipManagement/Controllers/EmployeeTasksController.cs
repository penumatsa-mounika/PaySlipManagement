using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTasksController : ControllerBase
    {
        private readonly IEmployeeTasksBALRepo _employeeTasksBALRepo;
        public EmployeeTasksController(IEmployeeTasksBALRepo employeeTasksBALRepo)
        {
            _employeeTasksBALRepo = employeeTasksBALRepo;
        }
        [HttpGet("GetAllEmployeeTasks")]
        public async Task<IEnumerable<EmployeeTasks>> GetAllEmployeeTasksAsync()
        {
            return await _employeeTasksBALRepo.GetAllEmployeeTasksAsync();
        }
        [HttpGet("GetEmployeeTasksByid/{id}")]
        public async Task<EmployeeTasks> GetEmployeeTasksByidAsync(int id)
        {
            EmployeeTasks _employeeTasks = new EmployeeTasks();
            _employeeTasks.Id = id;
            return await _employeeTasksBALRepo.GetEmployeeTasksByidAsync(_employeeTasks);
        }
        [HttpGet("GetEmployeeTasksByEmpCode/{Emp_Code}/{durationfilter}")]
        public async Task<IEnumerable<EmployeeTasks>> GetEmployeeTasksByCodeAsync(string Emp_Code, string durationfilter)
        {
            return await _employeeTasksBALRepo.GetEmployeeTasksByCodeAsync(Emp_Code, durationfilter);
        }
        [HttpPost("CreateEmployeeTasks")]
        public async Task<bool> CreateEmployeeTasks(EmployeeTasks _employeeTasks)
        {
            return await _employeeTasksBALRepo.CreateEmployeeTasks(_employeeTasks);

        }
        [HttpPut("UpdateEmployeeTasks")]
        public async Task<bool> UpdateEmployeeTasks(EmployeeTasks _employeeTasks)
        {
            return await _employeeTasksBALRepo.UpdateEmployeeTasks(_employeeTasks);

        }
        [HttpGet("DeleteEmployeeTasks/{id}")]
        public async Task<bool> DeleteEmployeeTasks(int id)
        {
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Id = id;
            return await _employeeTasksBALRepo.DeleteEmployeeTasks(employeeTasks);
        }
    }
}
