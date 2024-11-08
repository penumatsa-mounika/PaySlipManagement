using Microsoft.AspNetCore.Mvc;
using PayslipManagement.Common.Models;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : Controller
    {
        ILeavesBALRepo _leavesBALRepo;
        public LeavesController(ILeavesBALRepo leavesBALRepo)
        {
            _leavesBALRepo = leavesBALRepo;
        }
        [HttpGet("GetAllLeaves")]
        public async Task<IEnumerable<Leaves>> GetAllLeavesAsync()
        {
            return await _leavesBALRepo.GetAllLeavesAsync();
        }
        [HttpGet("GetLeavesByid/{id}")]
        public async Task<Leaves> GetLeavesByidAsync(int id)
        {
            Leaves _leaves = new Leaves();
            _leaves.Id = id;
            return await _leavesBALRepo.GetLeavesByidAsync(_leaves);
        }
        [HttpGet("GetLeavesByEmpCode/{Emp_Code}")]
        public async Task<Leaves> GetLeavesByCodeAsync(string Emp_Code)
        {
            return await _leavesBALRepo.GetLeavesByCodeAsync(Emp_Code);
        }
        [HttpPost("CreateLeaves")]
        public async Task<bool> CreateLeaves(Leaves _leaves)
        {
            return await _leavesBALRepo.CreateLeaves(_leaves);

        }
        [HttpPut("UpdateLeaves")]
        public async Task<bool> UpdateLeaves(Leaves _leaves)
        {
            return await _leavesBALRepo.UpdateLeaves(_leaves);

        }
        [HttpGet("DeleteLeaves/{id}")]
        public async Task<bool> DeleteLeaves(int id)
        {
            Leaves leaves = new Leaves();
            leaves.Id = id;
            return await _leavesBALRepo.DeleteLeaves(leaves);
        }

        [HttpPost("CarryForwardLeaves")]
        public async Task<IActionResult> CarryForwardLeaves()
        {
            await _leavesBALRepo.CarryForwardLeavesAsync();
            return Ok("Leaves carried forward successfully");
        }

        [HttpPost("MonthlyLeaveAddition")]
        public async Task<IActionResult> MonthlyLeaveAddition()
        {
            await _leavesBALRepo.MonthlyLeaveAdditionAsync();
            return Ok("Monthly Additon is done succesfully");
        }
    }
}
