using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.DAL.DapperServices.Implementations;
using PaySlipManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PaySlipManagement.DAL.Implementations
{
    public class LeavesDALRepo:ILeavesDALRepo
    {
        DapperServices<Leaves> leavesRepository;
        public LeavesDALRepo()
        {
            leavesRepository = new DapperServices<Leaves>();
        }


        public async Task<IEnumerable<Leaves>> GetAllLeavesAsync()
        {
            try
            {
                var result = await leavesRepository.ReadAllAsync(new Leaves() { Id = null });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Leaves> GetLeavesByidAsync(Leaves _leaves)
        {
            try
            {
                return await leavesRepository.ReadGetByIdAsync(_leaves);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Leaves> GetLeavesByCodeAsync(string Emp_Code)
        {
            try
            {
                Leaves l = new Leaves();
                l.Emp_Code = Emp_Code;
                DapperServices<Leaves> _leavesRepo = new DapperServices<Leaves>();
                return await _leavesRepo.ReadGetByAllCodeAsync(l);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateLeaves(Leaves _leaves)
        {
            try
            {

                if (_leaves != null)
                {
                    var employeeExists = await leavesRepository.CheckEmployeeExistsAsync(_leaves.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await leavesRepository.CreateAsync(_leaves);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateLeaves(Leaves _leaves)
        {
            try
            {
                if (_leaves != null)
                {
                    //check employee exist code
                    var employeeExists = await leavesRepository.CheckEmployeeExistsAsync(_leaves.Emp_Code);
                    if (!employeeExists)
                    {
                        return false; // Employee does not exist, creation cannot proceed
                    }
                    await leavesRepository.UpdateAsync(_leaves);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> DeleteLeaves(Leaves leaves)
        {
            try
            {
                if (leaves != null)
                {
                    await leavesRepository.DeleteAsync(leaves);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        

        public async Task CarryForwardLeavesAsync()
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
                            // new year
                            leave.CarriedForwardLeaves = Math.Min(unusedLeaves, 12); 
                            
                            //Jan increment if running in Dec
                            if(DateTime.Now.Month == 12)
                            {
                                leave.TotalLeaves = leave.CarriedForwardLeaves.Value + 1.5m;
                            }
                            else
                            {
                                leave.TotalLeaves = leave.CarriedForwardLeaves.Value;
                            }
                            
                            leave.LeavesAvailable = leave.TotalLeaves;

                            //reset
                            leave.LeavesUsed = 0;
                            leave.Year = DateTime.Now.Year + 1;
                            leave.CarriedForwardLeaves = null;


                            await UpdateLeaves(leave);
                        }
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in CarryForwardLeavesAsync: " + ex.Message);
                    throw;
                }
            }
        }

        public async Task MonthlyLeaveAdditionAsync()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var allLeaves = await GetAllLeavesAsync();

                    foreach (var leave in allLeaves)
                    {
                        // Skip Jan if carryforward added increment on dec 31
                        if(DateTime.Now.Month == 1)
                        {
                            continue;
                        }

                        // Increment total leaves by 1.5 for the month
                        leave.TotalLeaves += 1.5m;
                        leave.LeavesAvailable = leave.TotalLeaves - leave.LeavesUsed;

                        await UpdateLeaves(leave);
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in MonthlyLeaveAdditonAsync: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
