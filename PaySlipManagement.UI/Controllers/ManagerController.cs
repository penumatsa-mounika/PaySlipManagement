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
        public async Task<IActionResult> Index()
        {
            var manager = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.ManagerViewModel>($"{_apiSettings.ManagerEndPoint}/GetAllManager");
            return View(manager);
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
