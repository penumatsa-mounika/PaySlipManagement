using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class SalaryMetadataController : Controller
    {
        private readonly APIServices _apiService;
        private readonly ApiSettings _apiSettings;
        public SalaryMetadataController(APIServices apiService, IOptions<ApiSettings> apiSettings)
        {
            _apiService = apiService;
            _apiSettings = apiSettings.Value;
        }
        // GET: SalaryMetadata
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            // Fetch all SalaryMetadata
            var salaryMetadataList = await _apiService.GetAllAsync<SalaryMetadataViewModel>($"{_apiSettings.SalaryEndpoint}/GetAllSalaryMetadata");

            // Calculate total number of items
            int totalItems = salaryMetadataList.Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Ensure current page is within bounds
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            // Calculate the number of items to skip
            int skipItems = (currentPage - 1) * pageSize;

            // Get the paginated data for the current page
            var pagedSalaryMetadata = salaryMetadataList.Skip(skipItems).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedSalaryMetadata);
        }

        // GET: SalaryMetadata/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync<SalaryMetadataViewModel>($"{_apiSettings.SalaryEndpoint}/GetSalaryMetadataById/{id}");
            return View(response);
        }
        // GET: SalaryMetadata/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaryMetadata/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalaryMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync($"{_apiSettings.SalaryEndpoint}/CreateSalaryMetadata", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: SalaryMetadata/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync<SalaryMetadataViewModel>($"{_apiSettings.SalaryEndpoint}/GetSalaryMetadataById/{id}");
            return View(response);
        }

        // POST: SalaryMetadata/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalaryMetadata model)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PutAsync($"{_apiSettings.SalaryEndpoint}/UpdateSalaryMetadata", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: SalaryMetadata/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.GetAsync<SalaryMetadataViewModel>($"{_apiSettings.SalaryEndpoint}/GetSalaryMetadataById/{id}");
            return View(response);
        }

        // POST: SalaryMetadata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await _apiService.GetAsync<bool>($"{_apiSettings.SalaryEndpoint}/DeleteSalaryMetadata/{id}");
            if (data==true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }
}
