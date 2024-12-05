using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class DocumentController : Controller
    {

        private readonly APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public DocumentController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            // Fetch the latest holiday PDF
            var doc = await _apiServices.GetAsync<DocumentViewModel>($"{_apiSettings.DocumentEndpoint}/GetDocumentsByIdAsync");

            return View(doc);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(IFormFile fileImage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (fileImage != null && fileImage.Length > 0)
        //        {
        //            var image = new HolidayImage
        //            {
        //                Id = 0,
        //                ImageName = fileImage.FileName,
        //                ContentType = fileImage.ContentType
        //            };

        //            using (var stream = new MemoryStream())
        //            {
        //                await fileImage.CopyToAsync(stream);
        //                image.ImageData = stream.ToArray();
        //            }
        //            var response = await _apiServices.PostAsync($"{_apiSettings.DocumentEndpoint}/CreateDocumentAsync", image);
        //            if (!string.IsNullOrEmpty(response) && response == "Role Registered Successfully" || response == "true")
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //            else
        //            {
        //                //Handle the case where the API request fails or register is unsuccessful
        //                if (response != null)
        //                {
        //                    ModelState.AddModelError(string.Empty, response);
        //                }
        //                ModelState.AddModelError(string.Empty, "API request failed or Create was unsuccessful");
        //            }
        //        }
        //    }
        //    //return View(model);

        //    ModelState.AddModelError(string.Empty, "Invalid Create attempt");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Upload(List<IFormFile> files, string empCode)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Check if any files were uploaded
        //        if (files == null || files.Count == 0)
        //        {
        //            ModelState.AddModelError(string.Empty, "Please upload at least one valid file.");
        //            return View("Create");
        //        }

        //        foreach (var file in files)
        //        {
        //            // Validate the file input
        //            if (file == null || file.Length == 0)
        //            {
        //                ModelState.AddModelError(string.Empty, "Please upload a valid file.");
        //                return View("Create");
        //            }

        //            // Validate the file format (only PDFs allowed)
        //            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
        //            {
        //                ModelState.AddModelError(string.Empty, "Only PDF files are allowed.");
        //                return View("Create");
        //            }

        //            // Create the document model
        //            var document = new Document
        //            {
        //                Id = 0, // Assuming 0 for new records
        //                FileName = file.FileName,
        //                DocumentType = GetDocumentType(file.FileName), // You may need to implement this method to determine the document type
        //                Emp_Code = empCode
        //            };

        //            // Read the file data
        //            using (var stream = new MemoryStream())
        //            {
        //                await file.CopyToAsync(stream);
        //                document.FileData = stream.ToArray();
        //            }

        //            // Call the API to save the document
        //            var response = await _apiServices.PostAsync($"{_apiSettings.DocumentEndpoint}/CreateDocumentAsync", document);

        //            // Handle the response from the API
        //            if (string.IsNullOrEmpty(response) || !(response == "Document Uploaded Successfully" || response == "true"))
        //            {
        //                // Add appropriate error messages
        //                if (!string.IsNullOrEmpty(response))
        //                {
        //                    ModelState.AddModelError(string.Empty, response);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, "Failed to upload document. Please try again.");
        //                }
        //                return View("Create");
        //            }
        //        }

        //        TempData["SuccessMessage"] = "Documents uploaded successfully.";
        //        return RedirectToAction(nameof(Create));
        //    }

        //    // If we reach here, something went wrong
        //    ModelState.AddModelError(string.Empty, "Invalid upload attempt.");
        //    return View("Create");
        //}

        //// Helper method to determine document type based on file name or other criteria
        //private string GetDocumentType(string fileName)
        //{
        //    // Implement logic to determine document type based on the file name or other criteria
        //    // For example, you could use a dictionary or switch statement to map file names to document types
        //    return "Some Document Type"; // Replace with actual logic
        //}

        //[HttpGet]
        //[Route("Document/Create")] // Specify a unique route for the GET method
        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Route("Document/Create")] // Specify the same route for the POST method
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(List<IFormFile> files, List<string> documentTypes, string empCode)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Check if any files were uploaded
        //        if (files == null || files.Count == 0)
        //        {
        //            ModelState.AddModelError(string.Empty, "Please upload at least one valid file.");
        //            return View("Create");
        //        }

        //        // Check if the number of files matches the number of document types
        //        if (files.Count != documentTypes.Count)
        //        {
        //            ModelState.AddModelError(string.Empty, "The number of files must match the number of document types.");
        //            return View("Create");
        //        }

        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            var file = files[i];
        //            var documentType = documentTypes[i]; // Get the corresponding document type

        //            // Validate the file input
        //            if (file == null || file.Length == 0)
        //            {
        //                ModelState.AddModelError(string.Empty, "Please upload a valid file.");
        //                return View("Create");
        //            }

        //            // Validate the file format (only PDFs allowed)
        //            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
        //            {
        //                ModelState.AddModelError(string.Empty, "Only PDF files are allowed.");
        //                return View("Create");
        //            }

        //            // Create the document model
        //            var document = new Document
        //            {
        //                Id = 0, // Assuming 0 for new records
        //                FileName = file.FileName,
        //                DocumentType = documentType, // Use the document type from the hidden input
        //                Emp_Code = empCode
        //            };

        //            // Read the file data
        //            using (var stream = new MemoryStream())
        //            {
        //                await file.CopyToAsync(stream);
        //                document.FileData = stream.ToArray();
        //            }

        //            // Call the API to save the document
        //            var response = await _apiServices.PostAsync($"{_apiSettings.DocumentEndpoint}/CreateDocumentAsync", document);

        //            // Handle the response from the API
        //            if (string.IsNullOrEmpty(response) || !(response == "Document Uploaded Successfully" || response == "true"))
        //            {
        //                // Add appropriate error messages
        //                if (!string.IsNullOrEmpty(response))
        //                {
        //                    ModelState.AddModelError(string.Empty, response);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, "Failed to upload document. Please try again.");
        //                }
        //                return View("Create");
        //            }
        //        }

        //        TempData["SuccessMessage"] = "Documents uploaded successfully.";
        //        return RedirectToAction(nameof(Create)); // Redirect to the Create action after successful upload
        //    }

        //    // If we reach here, something went wrong
        //    ModelState.AddModelError(string.Empty, "Invalid upload attempt.");
        //    return View("Create");
        //}
        [HttpGet]
        [Route("document/create")] // Specify a unique route for the GET method
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("document/create")] // Specify the same route for the POST method
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, List<string> documentTypes, string empCode)
        {
            if (ModelState.IsValid)
            {
                // Check if any files were uploaded
                if (files == null || files.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please upload at least one valid file.");
                    return View("Create");
                }

                // Check if the number of files matches the number of document types
                if (files.Count != documentTypes.Count)
                {
                    ModelState.AddModelError(string.Empty, "The number of files must match the number of document types.");
                    return View("Create");
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var documentType = documentTypes[i]; // Get the corresponding document type

                    // Validate the file input
                    if (file == null || file.Length == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Please upload a valid file.");
                        return View("Create");
                    }

                    // Validate the file format (only PDFs allowed)
                    if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(string.Empty, "Only PDF files are allowed.");
                        return View("Create");
                    }

                    // Create the document model
                    var document = new Document
                    {
                        Id = 0, // Assuming 0 for new records
                        FileName = file.FileName,
                        DocumentType = documentType, // Use the document type from the hidden input
                        Emp_Code = empCode
                    };

                    // Read the file data
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        document.FileData = stream.ToArray();
                    }

                    // Call the API to save the document
                    var response = await _apiServices.PostAsync($"{_apiSettings.DocumentEndpoint}/CreateDocumentAsync", document);

                    // Handle the response from the API
                    if (string.IsNullOrEmpty(response) || !(response == "Document Uploaded Successfully" || response == "true"))
                    {
                        // Add appropriate error messages
                        if (!string.IsNullOrEmpty(response))
                        {
                            ModelState.AddModelError(string.Empty, response);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to upload document. Please try again.");
                        }
                        return View("Create");
                    }
                }

                TempData["SuccessMessage"] = "Documents uploaded successfully.";
                return RedirectToAction(nameof(Create)); // Redirect to the Create action after successful upload
            }

            // If we reach here, something went wrong
            ModelState.AddModelError(string.Empty, "Invalid upload attempt.");
            return View("Create");
        }
    }
}
