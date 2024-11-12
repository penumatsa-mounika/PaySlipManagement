using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.Threading.Tasks;

namespace PaySlipManagement.UI.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public LeaveRequestsController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(string ApprovalPerson, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            ApprovalPerson = empCode;
            // Fetch all leave requests
            var leaveRequests = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByManager/{ApprovalPerson}");

            // Filter only "Pending" requests
            var pendingRequests = leaveRequests?.Where(r => r.Status == "Pending").ToList();

            // Pagination logic
            var totalPending = pendingRequests.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = pendingRequests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(pagedPendingRequests);
        }

        public async Task<IActionResult> Index1(string Emp_Code)
        {
            var empCode = Request.Cookies["empCode"];
            Emp_Code = empCode;
            var leaveRequests = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByEmpCode/{Emp_Code}");
            return View(leaveRequests);
        }
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequests leaveRequests)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiServices.PostAsync<LeaveRequests>($"{_apiSettings.LeaveRequestsEndpoint}/CreateLeaveRequests", leaveRequests);
                if (response != null && response == "true")
                {
                    return RedirectToAction("Index");
                }
                return View(leaveRequests);
            }
            return View(leaveRequests);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequests model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.LeaveRequestsEndpoint}/DeleteLeaveRequests/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");

            if (response != null)
            {
                var leaveRequest = response;

                if (leaveRequest.Status == "Pending")
                {
                    // Calculate the requested leave days
                    var leaveDaysCount = 0;
                    if (leaveRequest.FromDate != null && leaveRequest.ToDate != null)
                    {
                        leaveDaysCount = (leaveRequest.ToDate - leaveRequest.FromDate).Value.Days + 1;
                    }
                    leaveRequest.LeavesCount = leaveDaysCount;

                    // Retrieve employee's leave balance
                    var empCode = leaveRequest.Emp_Code;
                    var leavesData = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByEmpCode/{empCode}");

                    if (leavesData == null)
                    {
                        return Json(new { success = false, message = "Employee leave data not found." });
                    }

                    // Check if requested days exceed available balance
                    if (leaveDaysCount > leavesData.LeavesAvailable)
                    {
                        // Redirect to LeavesLOP if excess leave days are requested
                        return await LeavesLOP(id);
                    }

                    // If within available balance, approve the request directly without LOP calculation
                    leaveRequest.Status = "Approved";
                    await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", leaveRequest);
                    // Update the leaves available
                    leavesData.LeavesAvailable -= leaveDaysCount;
                    await _apiServices.PutAsync($"{_apiSettings.LeavesEndpoint}/UpdateLeaves", leavesData);

                    return Json(new { success = true, message = "Request approved successfully!" });
                }
            }

            return Json(new { success = false, message = "An error occurred while approving the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
            if (response != null)
            {
                var model = response;
                if (model.Status == "Pending")
                {
                    model.Status = "Declined";
                    await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", model);
                    return Json(new { success = true, message = "Request canceled successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while canceling the request." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave(LeaveRequests leaveRequests, bool isConfirmed = false)
        {
            try
            {
                var empCode = Request.Cookies["empCode"];
                leaveRequests.Emp_Code = empCode;

                // Retrieve the employee details to find their manager
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");

                if (employee != null)
                {
                    leaveRequests.ApprovalPerson = employee.ManagerCode;
                    leaveRequests.Status = "Pending";

                    // Calculate the number of requested leave days
                    var count = 0;
                    if (leaveRequests.FromDate != null && leaveRequests.ToDate != null)
                    {
                        count = (leaveRequests.ToDate - leaveRequests.FromDate).Value.Days + 1;
                    }
                    leaveRequests.LeavesCount = count;

                    // Retrieve leave balance data for the employee
                    var leavesData = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByEmpCode/{empCode}");

                    if (leavesData != null && count > leavesData.LeavesAvailable && !isConfirmed)
                    {
                        // Show a confirmation message if extra leaves are requested
                        TempData["ConfirmationMessage"] = "You are requesting more leaves than your available balance. Extra days will be treated as paid leave. Do you wish to continue?";
                        TempData["ShowConfirmationPopup"] = true;
                        TempData["LeaveRequest"] = leaveRequests;
                        return RedirectToAction("Index1");
                    }

                    // Proceed with the leave request submission
                    var response = await _apiServices.PostAsync<LeaveRequests>($"{_apiSettings.LeaveRequestsEndpoint}/CreateLeaveRequests", leaveRequests);

                    if (!string.IsNullOrEmpty(response))
                    {
                        return RedirectToAction("Index1");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to submit leave request.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Employee not found.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
            return View(Index1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeavesLOP(int id)
        {
            try
            {
                // Retrieve the leave request, employee's leave balance and salary metadata
                var leaveRequest = await _apiServices.GetAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByid/{id}");
                var empCode = leaveRequest.Emp_Code;
                var leavesData = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByEmpCode/{empCode}");
                var salaryMetadata = await _apiServices.GetAsync<SalaryMetadataViewModel>($"{_apiSettings.SalaryEndpoint}/GetSalaryMetadataByid/{id}");

                if (leavesData == null || salaryMetadata == null)
                {
                    return Json(new { success = false, message = "Employee leave or salary data not found." });
                }

                // Calculate excess leaves and apply LOP if the request exceeds available balance
                var excessLeaves = leaveRequest.LeavesCount - leavesData.LeavesAvailable;
                if (excessLeaves > 0)
                {
                    decimal currentAbsentDays = salaryMetadata.AbsentDays;
                    decimal additionalAbsentDays = Convert.ToDecimal(excessLeaves);
                    salaryMetadata.AbsentDays = currentAbsentDays + additionalAbsentDays;
                    await _apiServices.PutAsync($"{_apiSettings.SalaryEndpoint}/UpdateSalaryMetadata", salaryMetadata);
                }
                leaveRequest.Status = "Approved";// Update the leave request status
                await _apiServices.PutAsync($"{_apiSettings.LeaveRequestsEndpoint}/UpdateLeaveRequests", leaveRequest);

                leavesData.LeavesAvailable = 0; // Update the leaves available & Set to 0 as all available leaves are used
                await _apiServices.PutAsync($"{_apiSettings.LeavesEndpoint}/UpdateLeaves", leavesData);

                return Json(new { success = true, message = "Request approved with LOP calculation." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}
