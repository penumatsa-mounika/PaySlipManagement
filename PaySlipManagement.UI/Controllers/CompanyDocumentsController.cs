using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        public async Task<IActionResult> Index()
        {
            return View();
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


                var document = new CompanyDocuments
                {
                    Id = 0,
                    FileName = files.FileName,
                    DocumentType = documentType,
                    FileType = fileType, // Use the actual file content type
                    Emp_Code = empCode
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
                return RedirectToAction("Index", "Employee");
            }
            return View("Create");
        }
    }
}