using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class ManagerController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public ManagerController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            // Fetch all department data
            var manager = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.ManagerViewModel>($"{_apiSettings.ManagerEndPoint}/GetAllManager");

            // Calculate total number of items
            int totalItems = manager.Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Ensure current page is within bounds
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            // Calculate the number of items to skip
            int skipItems = (currentPage - 1) * pageSize;

            // Get the paginated department data for the current page
            var pagedManager = manager.Skip(skipItems).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            // ViewData for toast message
            ViewData["ToastMessage"] = "Retrieved all Manager details.";

            // Return the paginated data to the view
            return View(pagedManager);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<ManagerViewModel>($"{_apiSettings.ManagerEndPoint}/GetManagerById/{id}");
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manager _manager)
        {
            if (ModelState.IsValid)
            {
                ManagerViewModel m = new ManagerViewModel();
                m.Id = _manager.Id;
                m.Emp_Code = _manager.Emp_Code;
                m.ManagerCode = _manager.ManagerCode;
                var response = await _apiServices.PostAsync<Manager>($"{_apiSettings.ManagerEndPoint}/CreateManager", _manager);
                if (response != null && response == "true")
                {
                    return RedirectToAction("Index");
                }
                return View(m);
            }
            return View(_manager);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<ManagerViewModel>($"{_apiSettings.ManagerEndPoint}/GetManagerById/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manager model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.ManagerEndPoint}/UpdateManager", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<ManagerViewModel>($"{_apiSettings.ManagerEndPoint}/GetManagerById/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.ManagerEndPoint}/DeleteManager/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }
}
