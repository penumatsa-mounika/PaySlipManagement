using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerBALRepo _managerBALRepo;
        public ManagerController(IManagerBALRepo managerBALRepo)
        {
            _managerBALRepo = managerBALRepo;
        }
        [HttpGet("GetAllManager")]
        public async Task<IEnumerable<Manager>> GetAllManagerAsync()
        {
            return await _managerBALRepo.GetAllManagerAsync();
        }
        [HttpGet("GetManagerById/{id}")]
        public async Task<Manager> GetManagerByidAsync(int id)
        {
            Manager _manager = new Manager();
            _manager.Id = id;
            return await _managerBALRepo.GetManagerByidAsync(_manager);
        }
        [HttpPost("CreateManager")]
        public async Task<bool> CreateManager(Manager _manager)
        {
            return await _managerBALRepo.CreateManager(_manager);

        }
        [HttpPut("UpdateManager")]
        public async Task<bool> UpdateManager(Manager _manager)
        {
            return await _managerBALRepo.UpdateManager(_manager);

        }
        [HttpGet("DeleteManager/{id}")]
        public async Task<bool> DeleteManager(int id)
        {
            Manager manager = new Manager();
            manager.Id = id;
            return await _managerBALRepo.DeleteManager(manager);
        }
    }
}
