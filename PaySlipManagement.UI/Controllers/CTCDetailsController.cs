using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class CTCDetailsController : Controller
    {
        private readonly APIServices _apiService;
        private readonly ApiSettings _apiSettings;
        public CTCDetailsController(APIServices apiService, IOptions<ApiSettings> apiSettings)
        {
            _apiService = apiService;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            var response = await _apiService.GetAllAsync<CTCDetailsViewModel>($"{_apiSettings.CTCDetailsEndpoint}/GetAllCTCDetails");
            int totalItems = response.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            int skipItems = (currentPage - 1) * pageSize;

            var pagedCTCDetails = response.Skip(skipItems).Take(pageSize).ToList();

            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedCTCDetails);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync<CTCDetailsViewModel>($"{_apiSettings.CTCDetailsEndpoint}/GetCTCDetailsByid/{id}");
            return View(response);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CTCDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync($"{_apiSettings.CTCDetailsEndpoint}/CreateCTCDetails", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync<CTCDetailsViewModel>($"{_apiSettings.CTCDetailsEndpoint}/GetCTCDetailsByid/{id}");
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CTCDetails model)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PutAsync($"{_apiSettings.CTCDetailsEndpoint}/UpdateCTCDetails", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.GetAsync<CTCDetailsViewModel>($"{_apiSettings.CTCDetailsEndpoint}/GetCTCDetailsByid/{id}");
            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await _apiService.GetAsync<bool>($"{_apiSettings.CTCDetailsEndpoint}/DeleteCTCDetails/{id}");
            if (data == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }
}
