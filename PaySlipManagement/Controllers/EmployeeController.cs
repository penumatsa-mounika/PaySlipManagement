using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.Common.Models;
using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.DAL.Interfaces;
using PayslipManagement.Common.Models;
using PaySlipManagement.DAL.Implementations;
using Microsoft.Data.SqlClient;


namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeBALRepo _employeeBALRepo;
        public EmployeeController(IEmployeeBALRepo _empBALRepo)
        {
            _employeeBALRepo = _empBALRepo;
        }
        [HttpGet("GetAllEmployees")]
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeBALRepo.GetAllEmployees();
        }
        [HttpGet("GetEmployeeById/{id}")]
        public async Task<Employee> GetEmployeeByidAsync(int id)
        {
            Employee emp = new Employee();
            emp.Id = id;
            return await _employeeBALRepo.GetEmployeeById(emp);
        }
        [HttpGet("GetAllEmployeesDetails")]
        public async Task<IEnumerable<EmployeeDetails>> GetAllEmployeesDetailsAsync()
        {
            return await _employeeBALRepo.GetAllEmployeesDetailsAsync();
        }
        [HttpGet("GetEmployeeByEmpCode/{empcode}")]
        public async Task<EmployeeDetails> GetEmployeeByCodeAsync(string empcode)
        {
            return await _employeeBALRepo.GetEmployeeByCodeAsync(empcode);
        }
        [HttpGet("GetEmployeeDetailsByEmpCode/{empcode}")]
        public async Task<EmployeeDetails> GetEmployeeDetailsByCodeAsync(string empcode)
        {
            return await _employeeBALRepo.GetEmployeeDetailsByCodeAsync(empcode);
        }
        [HttpGet("GetEmployeeByEmpCode/{empcode}/{payperiod}")]
        public async Task<EmployeeDetails> GetEmployeeByidAsync(string empcode, string payperiod)
        {
            return await _employeeBALRepo.GetEmployeeByCodeAsync(empcode, payperiod);
        }
        [HttpPost("CreateEmployee")]
        public async Task<bool> Create(Employee _employee)
        {
            //return await _employeeBALRepo.CreateEmployee(_employee);
                return await _employeeBALRepo.AddEmployee(_employee);   

        }
        [HttpPut("UpdateEmployee")]
        public async Task<bool> Update(Employee _employee)
        {
            return await _employeeBALRepo.UpdateEmployee(_employee);

        }
        [HttpGet("DeleteEmployee/{id}")]
        public async Task<bool> Delete(int id)
        {
            Employee emp = new Employee();
            emp.Id = id;
            return await _employeeBALRepo.DeleteEmployee(emp);
        }

        [HttpPost("BulkInsertEmployees")]
        public async Task<ActionResult<object>> BulkInsertEmployees([FromBody] List<Employee> employees)
        {
            if (employees == null || employees.Count == 0)
            {
                return BadRequest(new { Message = "Employee list cannot be empty." });
            }

            try
            {
                bool isInserted = await _employeeBALRepo.BulkInsertEmployees(employees);

                if (!isInserted)
                {
                    return StatusCode(500, new { Message = "Bulk insert operation failed." });
                }

                return Ok(new { Message = "Employees inserted successfully." });
            }
            catch (SqlException sqlEx)
            {
                // Log SQL-specific error (if logging is available)
                Console.WriteLine($"SQL Error: {sqlEx.Message}");

                return StatusCode(500, new
                {
                    Message = "Database error occurred while inserting employees.",
                    Error = sqlEx.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");

                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while inserting employees.",
                    Error = ex.Message
                });
            }
        }



    }
}
