using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayslipManagement.Common.Models;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class CompanyDocumentsController : Controller
    {
        private readonly APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public CompanyDocumentsController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            ViewBag.SelectedEmpCode = null;
            ViewBag.SelecteddocumentType = null;

            return View(Enumerable.Empty<CompanyDocumentsViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Emp_Code, string DocumentType)
        {
            var employees = await _apiServices.GetAllAsync<EmployeeViewModel>($"{_apiSettings.EmployeeEndpoint}/GetAllEmployees");
            ViewBag.Employees = employees;

            // Pass the selected values back to the view
            ViewBag.SelectedEmpCode = Emp_Code;
            ViewBag.SelecteddocumentType = DocumentType;

            // Fetch employee tasks based on filters
            var response = await _apiServices.GetAllAsync<CompanyDocumentsViewModel>($"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsByIdAsync/{Emp_Code}/{DocumentType}");

            // Return the filtered tasks to the view
            return View(response);
        }

        public async Task<IActionResult> Documents(string Emp_Code, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            Emp_Code = empCode;
            var doc = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.CompanyDocumentsViewModel>($"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsByCode/{Emp_Code}");

            var totalPending = doc.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = doc.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(doc);
        }

        public async Task<IActionResult> Index1(string ApprovalPerson, int page = 1, int pageSize = 8)
        {
            var empCode = Request.Cookies["empCode"];
            ApprovalPerson = empCode;
            var request = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.CompanyDocumentsViewModel>($"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsByManager/{ApprovalPerson}");

            // Filter only "Pending" requests
            var pendingRequests = request?.Where(r => r.Status == "Pending").ToList();

            // Pagination logic
            var totalPending = pendingRequests.Count();
            var totalPages = (int)Math.Ceiling(totalPending / (double)pageSize);
            var pagedPendingRequests = pendingRequests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(pagedPendingRequests);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadDocument(int id)
        {

            var url = $"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsById/{id}";
            var doc = await _apiServices.GetAsync<CompanyDocumentsViewModel>(url);

            if (doc == null || doc.FileData == null)
            {
                return NotFound("Document not found.");
            }

            return File(doc.FileData, "application/octet-stream", doc.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocumentGet(int id)
        {
            return await ViewDocument(id); // Call the existing ViewDocument method
        }


        [HttpPost]
        public async Task<IActionResult> ViewDocument(int id)
        {
            var url = $"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsById/{id}";
            var doc = await _apiServices.GetAsync<CompanyDocumentsViewModel>(url);

            if (doc == null || doc.FileData == null)
            {
                return NotFound("Document not found.");
            }

            // Determine the MIME type based on the document type
            string mimeType;
            switch (Path.GetExtension(doc.FileName)?.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                default:
                    mimeType = "application/pdf";
                    break;
            }

            return File(doc.FileData, mimeType);
        }

        [HttpGet]
        //[Route("CompanyDocuments/create")] // Specify a unique route for the GET method
        public async Task<IActionResult> Create()
        {

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile files, string fileType, string documentType, string? empCode)
        {
            empCode = Request.Cookies["empCode"];
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(empCode))
                {
                    ModelState.AddModelError(string.Empty, "Employee code is required.");
                    return View("Create");
                }

                if (string.IsNullOrEmpty(documentType))
                {
                    ModelState.AddModelError(string.Empty, "Document type is required.");
                    return View("Create");
                }

                if (files == null || files.Length == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please upload a valid file.");
                    return View("Create");
                }

                var allowedFileTypes = new[] { "application/pdf", "image/jpeg", "image/png" };

                if (!allowedFileTypes.Contains(files.ContentType.ToLower()))
                {
                    ModelState.AddModelError(string.Empty, "Invalid file type. Only PDF, JPEG, and PNG are allowed.");
                    return View("Create");
                }
                var employee = await _apiServices.GetAsync<EmployeeDetails>(
                    $"{_apiSettings.EmployeeEndpoint}/GetEmployeeByEmpCode/{empCode}");

                var document = new CompanyDocuments
                {
                    Id = 0,
                    FileName = files.FileName,
                    DocumentType = documentType,
                    FileType = fileType, // Use the actual file content type
                    Emp_Code = empCode,
                    ApprovalPerson=employee.ManagerCode,
                    Status="Pending"
                };

                using (var stream = new MemoryStream())
                {
                    await files.CopyToAsync(stream);
                    document.FileData = stream.ToArray();
                }

                // API Call to Upload Document
                var response = await _apiServices.PostAsync($"{_apiSettings.CompanyDocumentsEndpoint}/Create", document);

                if (string.IsNullOrEmpty(response) || !(response == "Document Uploaded Successfully" || response == "true"))
                {
                    ModelState.AddModelError(string.Empty, response ?? "Failed to upload the document. Please try again.");
                    return View("Create");
                }

                TempData["SuccessMessage"] = "Document uploaded successfully.";
                TempData["ActiveTab"] = "upload-documents"; // Keep the tab active
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var response = await _apiServices.GetAsync<CompanyDocumentsViewModel>($"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsById/{id}");
            if (response != null)
            {
                var reg = response;

                if (reg.Status == "Pending")
                {
                    reg.Status = "Approved";
                    await _apiServices.PutAsync($"{_apiSettings.CompanyDocumentsEndpoint}/UpdateCompanyDocuments", reg);


                    return Json(new { success = true, message = "Request approved successfully!" });
                }
            }

            return Json(new { success = false, message = "An error occurred while approving the request." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var response = await _apiServices.GetAsync<CompanyDocumentsViewModel>($"{_apiSettings.CompanyDocumentsEndpoint}/GetCompanyDocumentsById/{id}");
            if (response != null)
            {
                var model = response;
                if (model.Status == "Pending")
                {
                    model.Status = "Declined";
                    await _apiServices.PutAsync($"{_apiSettings.CompanyDocumentsEndpoint}/UpdateCompanyDocuments", model);
                    return Json(new { success = true, message = "Request canceled successfully!" });
                }
            }
            return Json(new { success = false, message = "An error occurred while canceling the request." });
        }
    }
}