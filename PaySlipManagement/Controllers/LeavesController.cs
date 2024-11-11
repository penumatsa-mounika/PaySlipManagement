using Microsoft.AspNetCore.Mvc;
using PayslipManagement.Common.Models;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;
using System.Transactions;

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

        [HttpPost("carry-forward")]
        public async Task<IActionResult> CarryForwardLeavesAsync()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var allLeaves = await GetAllLeavesAsync();

                    foreach (var leave in allLeaves)
                    {
                        var unusedLeaves = leave.TotalLeaves - leave.LeavesUsed;

                        if (unusedLeaves > 0)
                        {
                            leave.CarriedForwardLeaves = Math.Min(unusedLeaves, 12);

                            //Jan increment if running in Dec
                            if (DateTime.Now.Month == 12)
                            {
                                leave.TotalLeaves = leave.CarriedForwardLeaves.Value + 1.5m;
                            }
                            
                            leave.LeavesAvailable = leave.TotalLeaves;
                            //reset
                            leave.LeavesUsed = 0;
                            leave.Year = DateTime.Now.Year + 1;
                            leave.CarriedForwardLeaves = null;

                            PaySlipManagement.DAL.DapperServices.Implementations.DapperServices<Leaves> db = new PaySlipManagement.DAL.DapperServices.Implementations.DapperServices<Leaves>();
                            await db.UpdateAsync(leave);
                        }
                    }
                    transaction.Complete();
                    return Ok("Carry forward successful");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }


        [HttpPost("monthly-addition")]
        public async Task<IActionResult> MonthlyLeaveAdditionAsync()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var allLeaves = await GetAllLeavesAsync();

                    foreach (var leave in allLeaves)
                    {
                        // Skip Jan if carryforward added increment on dec 31
                        if (DateTime.Now.Month == 1)
                        {
                            continue;
                        }

                        // Increment total leaves by 1.5 for the month
                        leave.TotalLeaves += 1.5m;
                        leave.LeavesAvailable = leave.TotalLeaves - leave.LeavesUsed;

                        PaySlipManagement.DAL.DapperServices.Implementations.DapperServices<Leaves> db = new PaySlipManagement.DAL.DapperServices.Implementations.DapperServices<Leaves>();
                        await db.UpdateAsync(leave);
                    }
                    transaction.Complete();
                    return Ok("Monthly addition successful");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }
    }
}
