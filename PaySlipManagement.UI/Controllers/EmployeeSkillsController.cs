using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeSkillsController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;

        public EmployeeSkillsController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(string ApprovalPerson, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            ApprovalPerson = empCode;
            // Fetch all data
            var skills = await _apiServices.GetAllAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByManager/{ApprovalPerson}");

            // Filter only "Pending" requests
            var pendingRequests = skills?.Where(r => r.Status == "Pending").ToList();
            // Pagination logic
            var totalPending = pendingRequests.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = pendingRequests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(pagedPendingRequests);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeSkills()
        {
            try
            {
                var response = await _apiServices.GetAllAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetAllEmployeeSkills"
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
            // Fetch list of employees for the dropdown
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            ViewBag.SelectedEmpCode = null;
            return View(Enumerable.Empty<EmployeeSkillsViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Index1(string Emp_Code)
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            // Pass the selected values back to the view
            ViewBag.SelectedEmpCode = Emp_Code;

            // Fetch employee skills
            var skills = await _apiServices.GetAllAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByCode/{Emp_Code}");

            // Filter skills to include only "Approved" ones
            //var approvedSkills = skills.Where(s => s.Status == "Approved").ToList();

            return View(skills);
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeTasksViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByid/{id}");
        //    return View(response);
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeSkills _Skills)
        {
            if (ModelState.IsValid)
            {
                EmployeeSkillsViewModel m = new EmployeeSkillsViewModel();
                m.Id = _Skills.Id;
                m.Emp_Code = _Skills.Emp_Code;

                return View(m);
            }
            return View(_Skills);
        }



        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByid/{id}");
        //    return View(response);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EmployeeSkills model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _apiServices.PutAsync($"{_apiSettings.EmployeeSkillsEndpoint}/UpdateEmployeeSkills", model);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}



        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _apiServices.GetAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/DeleteEmployeeSkills/{id}");
        //    return View(response);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var response = await _apiServices.GetAsync<bool>($"{_apiSettings.EmployeeSkillsEndpoint}/DeleteEmployeeSkills/{id}");
        //    if (response == true)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View("Delete");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByid/{id}");

            if (response != null)
            {
                var skill = response;

                if (skill.Status == "Pending")
                {
                    skill.Status = "Approved"; // Change status to Approved

                    await _apiServices.PutAsync($"{_apiSettings.EmployeeSkillsEndpoint}/UpdateEmployeeSkills", skill);

                    return Json(new { success = true, message = "Skill approved successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while approving the skill." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/GetEmployeeSkillsByid/{id}");

            if (response != null)
            {
                var skill = response;

                if (skill.Status == "Pending")
                {
                    skill.Status = "Declined"; // Change status to Declined

                    await _apiServices.PutAsync($"{_apiSettings.EmployeeSkillsEndpoint}/UpdateEmployeeSkills", skill);

                    return Json(new { success = true, message = "Skill request declined successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while declining the skill request." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitSkills(List<EmployeeSkillsViewModel> skills)
        {
            try
            {
                if (skills == null || !skills.Any())
                {
                    ModelState.AddModelError("", "No skills submitted. Please add skills and try again.");
                    return View("Index");
                }

                var empCode = Request.Cookies["empCode"];
                // Fetch employee details to get the approval person's (manager's) code
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");

                foreach (var skill in skills)
                {
                    skill.Emp_Code = empCode;
                    skill.Status = "Pending"; // Default status: Pending
                    skill.ApprovalPerson = employee.ManagerCode; // Assign manager as approver

                    if (string.IsNullOrEmpty(skill.SkillName) || skill.ProficiencyLevel == null || skill.YearsOfExperience < 0)
                    {
                        ModelState.AddModelError("", "Skill Name, Proficiency Level, and Years of Experience are required.");
                        return View("Index");
                    }
                    // Send each skill individually to the API
                    var response = await _apiServices.PostAsync<EmployeeSkillsViewModel>($"{_apiSettings.EmployeeSkillsEndpoint}/CreateEmployeeSkills", skill);

                    if (response == null)
                    {
                        ModelState.AddModelError("", "Failed to submit some skills. Please try again.");
                        return View("Index");
                    }
                }
                TempData["SuccessMessage"] = "Skills submitted successfully and are pending approval.";
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
