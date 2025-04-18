using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeExperienceController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;

        public EmployeeExperienceController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(string ApprovalPerson, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            ApprovalPerson = empCode;
            var skills = await _apiServices.GetAllAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/GetEmployeeExperienceByManager/{ApprovalPerson}");

            var pendingRequests = skills?.Where(r => r.Status == "Pending").ToList();
            var totalPending = pendingRequests.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = pendingRequests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(pagedPendingRequests);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeExperience()
        {
            try
            {
                var response = await _apiServices.GetAllAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/GetAllEmployeeExperience"
                );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index1()
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            ViewBag.SelectedEmpCode = null;
            return View(Enumerable.Empty<EmployeeExperienceViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Index1(string Emp_Code)
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            ViewBag.SelectedEmpCode = Emp_Code;

            var skills = await _apiServices.GetAllAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/GetEmployeeExperienceByCode/{Emp_Code}");


            return View(skills);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeExperience _Experience)
        {
            if (ModelState.IsValid)
            {
                EmployeeExperienceViewModel m = new EmployeeExperienceViewModel();
                m.Id = _Experience.Id;
                m.Emp_Code = _Experience.Emp_Code;

                return View(m);
            }
            return View(_Experience);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/GetEmployeeExperienceByid/{id}");

            if (response != null)
            {
                var skill = response;

                if (skill.Status == "Pending")
                {
                    skill.Status = "Approved"; 

                    await _apiServices.PutAsync($"{_apiSettings.EmployeeExperienceEndpoint}/UpdateEmployeeExperience", skill);

                    return Json(new { success = true, message = "Experience approved successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while approving the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/GetEmployeeExperienceByid/{id}");

            if (response != null)
            {
                var skill = response;

                if (skill.Status == "Pending")
                {
                    skill.Status = "Declined"; 

                    await _apiServices.PutAsync($"{_apiSettings.EmployeeExperienceEndpoint}/UpdateEmployeeExperience", skill);

                    return Json(new { success = true, message = "Experience request declined successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while declining the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExperience(List<EmployeeExperienceViewModel> experiences)
        {
            try
            {
                if (experiences == null || !experiences.Any())
                {
                    ModelState.AddModelError("", "No experience submitted. Please add skills and try again.");
                    return View("Index");
                }

                var empCode = Request.Cookies["empCode"];
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");

                foreach (var ex in experiences)
                {
                    ex.Emp_Code = empCode;
                    ex.Status = "Pending"; 
                    ex.ApprovalPerson = employee.ManagerCode; 

                    
                    var response = await _apiServices.PostAsync<EmployeeExperienceViewModel>($"{_apiSettings.EmployeeExperienceEndpoint}/CreateEmployeeExperience", ex);

                    if (response == null)
                    {
                        ModelState.AddModelError("", "Failed to submit some skills. Please try again.");
                        return View("Index");
                    }
                }
                TempData["SuccessMessage"] = "Experience submitted successfully and are pending approval.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Index");
            }
            return RedirectToAction("GeneratePdf", "Employee");
        }
    }
}