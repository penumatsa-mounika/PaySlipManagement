using iText.Html2pdf;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.Common.Utilities;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.Globalization;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeController : Controller
    {

        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;

        public EmployeeController(APIServices apiService, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiService;
            _apiSettings = apiSettings.Value;
        }

        //GET: EmployeeController

        public async Task<IActionResult> Index(int? departmentId, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            var emp = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            var departments = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.DepartmentViewModel>($"{_apiSettings.DepartmentEndpoint}/GetAllDepartments");
            var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");
            var roles = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.RolesViewModel>($"{_apiSettings.RolesEndpoint}/GetAllAsyncRoles");
            var userRoles = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.UserRolesViewModel>($"{_apiSettings.UserRoleEndpoint}/GetAllAsyncUserRoles");

            var employeeWithDepartmentList = emp.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                Emp_Code = e.Emp_Code,
                EmployeeName = e.EmployeeName,
                DepartmentId = e.DepartmentId,
                DepartmentName = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.DepartmentName,

                // Fetching Role by joining Employee, UserRoles, and Roles
                Role = (from ur in userRoles
                        join r in roles on ur.RoleId equals r.Id
                        where ur.Emp_Code == e.Emp_Code
                        select r.Role).ToList(),  // Fetching role as List<string>

                Designation = e.Designation,
                Division = e.Division,
                Email = e.Email,
                IsActive = e.IsActive,
                PhoneNumber = e.PhoneNumber,
                PAN_Number = e.PAN_Number,
                //JoiningDate = DateTime.TryParseExact(e.JoiningDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime joiningDateTime)
                    //? joiningDateTime.ToString("MM/dd/yyyy")
                    //: string.Empty,
                JoiningDate = e.JoiningDate,
            }).ToList();

            // Filter employees if departmentId is provided
            if (departmentId.HasValue)
            {
                employeeWithDepartmentList = employeeWithDepartmentList.Where(e => e.DepartmentId == departmentId.Value).ToList();
            }

            // Implement paging
            int totalItems = employeeWithDepartmentList.Count;
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            int currentPage = page > totalPages ? totalPages : page;
            int skipItems = (currentPage - 1) * pageSize;

            var pagedEmployeeList = employeeWithDepartmentList.Skip(skipItems).Take(pageSize).ToList();
            var paySlips = CalculatePaySlips(employee.JoiningDate, DateTime.Now);
            ViewBag.PaySlips = paySlips;
            ViewBag.Departments = new SelectList(departments, "Id", "DepartmentName");
            ViewBag.SelectedDepartmentId = departmentId;
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedEmployeeList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee();
                employee.Id = model.Id;
                employee.Emp_Code = model.Emp_Code;
                employee.EmployeeName = model.EmployeeName;
                employee.DepartmentId = model.DepartmentId;
                employee.Designation = model.Designation;
                employee.Division = model.Division;
                employee.Email = model.Email;
                employee.PAN_Number = model.PAN_Number;
                employee.JoiningDate = model.JoiningDate;
                employee.IsActive = model.IsActive;
                employee.PhoneNumber = model.PhoneNumber;

                // Make a POST request to the Web API
                var response = await _apiServices.PostAsync($"{_apiSettings.EmployeeEndpoint}/CreateEmployee", model);

                if (!string.IsNullOrEmpty(response) && response == "Employee Registered Successfully" || response == "true")
                {
                    TempData["message"] = response;
                    // Handle a successful Register
                    return RedirectToAction("Index");
                }
                else
                {

                    // Handle the case where the API request fails or register is unsuccessful
                    if (response != null)
                    {
                        ModelState.AddModelError(string.Empty, response);
                    }
                    ModelState.AddModelError(string.Empty, "API request failed or Create was unsuccessful");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Create attempt");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeById/{id}");
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                // Make a POST request to the Web API
                var response = await _apiServices.PutAsync<Employee>($"{_apiSettings.EmployeeEndpoint}/UpdateEmployee", model);

                if (!string.IsNullOrEmpty(response) && response == "Employee Updated Successfully" || response == "true")
                {
                    TempData["message"] = response;
                    // Handle a successful Updated
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where the API request fails or register is unsuccessful
                    if (response != null)
                    {
                        ModelState.AddModelError(string.Empty, response);
                    }
                    ModelState.AddModelError(string.Empty, "API request failed or Update was unsuccessful");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Update attempt");
            return View("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeById/{id}");
            var departments = await _apiServices.GetAsync<PaySlipManagement.UI.Models.DepartmentViewModel>($"{_apiSettings.DepartmentEndpoint}/GetDepartmentById/{data.DepartmentId}");
            data.DepartmentName = departments.DepartmentName;
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _apiServices.GetAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeById/{id}");
            var departments = await _apiServices.GetAsync<PaySlipManagement.UI.Models.DepartmentViewModel>($"{_apiSettings.DepartmentEndpoint}/GetDepartmentById/{data.DepartmentId}");
            data.DepartmentName = departments.DepartmentName;
            return View(data);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.EmployeeEndpoint}/DeleteEmployee/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
        [HttpGet]
        public async Task<IActionResult> GeneratePdf()
        {
            var empCode = Request.Cookies["empCode"];
            var leaverequests = await _apiServices.GetAllAsync<LeaveRequestsViewModel>($"{_apiSettings.LeaveRequestsEndpoint}/GetLeaveRequestsByEmpCode/{empCode}");
           

            var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");
            var holidayImage = await _apiServices.GetAsync<HolidayImageViewModel>($"{_apiSettings.HolidayEndpoint}/GetHolidayImageByIdAsync");
            var holidayPdf = await _apiServices.GetAsync<HolidayPdfViewModel>($"{_apiSettings.HolidayEndpoint}/GetHolidayPdfByIdAsync");
            var leaves = await _apiServices.GetAsync<LeavesViewModel>($"{_apiSettings.LeavesEndpoint}/GetLeavesByEmpCode/{empCode}");


            

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }
            if (leaves == null)
            {
                return NotFound("Leaves not found.");
            }

            var payPeriods = CalculatePayPeriods(employee.JoiningDate, DateTime.Now);

            var holiday = new HolidayImagePDFViewModel()
            {
                HolidayImage = holidayImage,
                HolidayPdf = holidayPdf,
            };
            var model = new EmployeePayPeriodsViewModel
            {
                Employee = employee,
                PayPeriods = payPeriods,
                Holiday = holiday,
                Leaves = leaves,
                LeaveRequests= leaverequests
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateEmployeePdf(string empCode, string payPeriod)
        {
            try
            {
                // Fetch employee details from API
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}/{payPeriod}");

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                // Create a unique directory to store generated PDFs
                string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(directory);

                // Generate the PDF
                string filePath = Path.Combine(directory, $"{employee.Emp_Code}_{employee.PaySlipForMonth}_PaySlip.pdf");
                GenerateEmployeePdf(employee, filePath);

                // Provide download link
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", $"{employee.Emp_Code}_{employee.PaySlipForMonth}_PaySlip.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEmployeeCTCPdf(string empCode)
        {
            try
            {
                // Fetch employee details from API
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeDetailsByEmpCode/{empCode}");

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                // Create a unique directory to store generated PDFs
                string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(directory);

                // Generate the PDF
                string filePath = Path.Combine(directory, $"{employee.Emp_Code}_CTCBreakDown.pdf");
                GenerateEmployeeCTCPdf(employee, filePath);

                // Provide download link
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", $"{employee.Emp_Code}_CTCBreakDown.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ViewEmployeePdf(string empCode, string payPeriod)
        {
            try
            {
                // Fetch employee details from API
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}/{payPeriod}");

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                // Create a unique directory to store generated PDFs
                string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(directory);

                // Generate the PDF
                string filePath = Path.Combine(directory, $"{employee.Emp_Code}_{employee.PaySlipForMonth}_PaySlip.pdf");
                GenerateEmployeePdf(employee, filePath);

                // Provide PDF for viewing in browser
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                Response.Headers.Add("Content-Disposition", $"inline; filename={employee.Emp_Code}_{employee.PaySlipForMonth}_PaySlip.pdf");

                // Clean up the temporary file after serving it
                Task.Run(() => System.IO.File.Delete(filePath));

                return File(fileBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ViewEmployeeCTCPdf(string empCode)
        {
            try
            {
                // Fetch employee details from API
                var employee = await _apiServices.GetAsync<EmployeeDetails>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeDetailsByEmpCode/{empCode}");

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                // Create a unique directory to store generated PDFs
                string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(directory);

                // Generate the PDF
                string filePath = Path.Combine(directory, $"{employee.Emp_Code}_CTCBreakDown.pdf");
                GenerateEmployeeCTCPdf(employee, filePath);

                // Provide PDF for viewing in browser
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                Response.Headers.Add("Content-Disposition", $"inline; filename={employee.Emp_Code}_CTCBreakDown.pdf");

                // Clean up the temporary file after serving it
                Task.Run(() => System.IO.File.Delete(filePath));

                return File(fileBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("Error");
            }
        }
        private void GenerateEmployeePdf(EmployeeDetails employee, string filePath)
        {
            string htmlContent = System.IO.File.ReadAllText("wwwroot/Payslip.html");
            htmlContent = htmlContent.Replace("{{payPeriod}}", employee.PaySlipForMonth)
                                     .Replace("{{empCode}}", employee.Emp_Code)
                                     .Replace("{{empName}}", employee.EmployeeName)
                                     .Replace("{{designation}}", employee.Designation)
                                     .Replace("{{department}}", employee.DepartmentName)
                                     .Replace("{{BnNo}}", employee.BankAccountNumber.ToString())
                                     .Replace("{{jod}}", employee.JoiningDate?.ToString("yyyy-MM-dd"))
                                     .Replace("{{bankName}}", employee.BankName)
                                     .Replace("{{panNo}}", employee.PAN_Number)
                                     .Replace("{{uanNo}}", employee.UANNumber.ToString())
                                     .Replace("{{pfNo}}", employee.PFAccountNumber)
                                     .Replace("{{lwp}}", employee.AbsentDays.ToString())
                                     .Replace("{{absentDays}}", employee.AbsentDays.ToString())
                                     .Replace("{{totalDays}}", employee.DaysPaid.ToString())
                                     .Replace("{{eb}}", employee.EarnedBasic.ToString("C"))
                                     .Replace("{{pfes}}", employee.PFEmployeeShare.ToString("C"))
                                     .Replace("{{hra}}", employee.HRA.ToString("C"))
                                     .Replace("{{pt}}", employee.ProfessionalTax.ToString("C"))
                                     .Replace("{{sa}}", employee.SpecialAllowance.ToString("C"))
                                     .Replace("{{tds}}", employee.TDS.ToString("C"))
                                     .Replace("{{eamounttotal}}", employee.EarningTotal.ToString("C"))
                                     .Replace("{{damounttotal}}", employee.TotalDeductions.ToString("C"))
                                     .Replace("{{netpay}}", employee.NetPay.ToString("C"))
                                     .Replace("{{location}}", employee.Division)
                                     .Replace("{{netpayword}}", NumberToWordsConverter.ConvertToWords(employee.NetPay) + " Rupees")
                                     .Replace("{{address}}", employee.CompanyAddress);

            // Pass the correct file path
            Generatepdf(filePath, htmlContent);
        }

        private void GenerateEmployeeCTCPdf(EmployeeDetails employee, string filePath)
        {
            string htmlContent = System.IO.File.ReadAllText("wwwroot/CTCbreakdown.html");
            htmlContent = htmlContent.Replace("{{empName}}", employee.EmployeeName)
                                     .Replace("{{designation}}", employee.Designation)
                                     .Replace("{{department}}", employee.DepartmentName)
                                     .Replace("{{gsalmon}}", employee.MonthGrossPay.ToString("C"))
                                     .Replace("{{agsal}}", employee.AnnualGrossPay.ToString("C"))
                                     .Replace("{{pfersmon}}", employee.PFEmployerShare.ToString("C"))
                                     .Replace("{{apfers}}", employee.PFEmployerShareAnnual.ToString("C"))
                                     .Replace("{{ctcmon}}", employee.CTCMonth.ToString("C"))
                                     .Replace("{{actc}}", employee.AnnualCTC.ToString("C"))
                                     .Replace("{{eb}}", employee.EarnedBasic.ToString("C"))
                                     .Replace("{{tds}}", employee.TDS.ToString("C"))
                                     .Replace("{{hra}}", employee.HRA.ToString("C"))
                                     .Replace("{{pt}}", employee.ProfessionalTax.ToString("C"))
                                     .Replace("{{sa}}", employee.SpecialAllowance.ToString("C"))
                                     .Replace("{{pfes}}", employee.PFEmployeeShare.ToString("C"))
                                     .Replace("{{oadd}}", employee.OtherAdditions.ToString("C"))
                                     .Replace("{{oded}}", employee.OtherDeductions.ToString("C"))
                                     .Replace("{{damounttotal}}", employee.TotalDeductions.ToString("C"))
                                     .Replace("{{netpay}}", employee.NetPay.ToString("C"));
            // Pass the correct file path
            Generatepdf(filePath, htmlContent);
        }
        private void Generatepdf(string filePath, string htmlContent)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    PdfWriter writer = new PdfWriter(stream);
                    HtmlConverter.ConvertToPdf(htmlContent, writer);
                }
            }
            catch (PdfException pdfEx)
            {
                // Log detailed PDF exception
                Console.WriteLine($"PDF Exception: {pdfEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
        private List<string> CalculatePayPeriods(DateTime? joiningDate, DateTime endDate)
        {
            var payPeriods = new List<string>();
            var currentDate = endDate.AddMonths(-1);
            var startDate = endDate.AddMonths(-6);

            while (currentDate >= startDate && (joiningDate == null || currentDate >= joiningDate))
            {
                payPeriods.Add(currentDate.ToString("MMMM-yyyy"));			
	              currentDate = currentDate.AddMonths(-1);
	        }
	          payPeriods.Reverse();
            return payPeriods;
        }
        private List<string> CalculatePaySlips(DateTime? joiningDate, DateTime presentDate)
        {
            var paySlips = new List<string>();
            var currentDate = presentDate.AddMonths(-1);

            while (currentDate >= joiningDate)
            {
                paySlips.Add(currentDate.ToString("MMMM-yyyy"));
                currentDate = currentDate.AddMonths(-1);
            }
            paySlips.Reverse();
            return paySlips;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetEmployeeInactive(int id)
        {
            var employee = await _apiServices.GetAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetEmployeeById/{id}");
            if (employee == null)
            {
                return NotFound();
            }
            employee.IsActive = false; // Set the employee's status to inactive
            var result = await _apiServices.PutAsync($"{_apiSettings.EmployeeEndpoint}/UpdateEmployee", employee);

            if (result == null)
            {
                ModelState.AddModelError("", "Failed to update employee status.");
                return View(employee); // Return with the current employee if the update failed
            }
            return RedirectToAction("Index");
        }
    }

}


