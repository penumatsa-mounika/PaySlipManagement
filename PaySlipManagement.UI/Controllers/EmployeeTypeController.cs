using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeTypeController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public EmployeeTypeController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            var employeeType = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.EmployeeTypeViewModel>($"{_apiSettings.EmployeeTypeEndpoint}/GetAllEmployeeType");
            return View(employeeType);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeTypeViewModel>($"{_apiSettings.EmployeeTypeEndpoint}/GetEmployeeTypeByid/{id}");
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeType _employeeType)
        {
            if (ModelState.IsValid)
            {
                EmployeeTypeViewModel et = new EmployeeTypeViewModel();
                et.Id = _employeeType.Id;
                et.EmpType = _employeeType.EmpType;
                et.LeaveAllocation = _employeeType.LeaveAllocation;
                var response = await _apiServices.PostAsync<EmployeeType>($"{_apiSettings.EmployeeTypeEndpoint}/CreateEmployeeType", _employeeType);
                if (response != null && response == "true")
                {
                    return RedirectToAction("Index");
                }
                return View(et);
            }
            return View(_employeeType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeTypeViewModel>($"{_apiSettings.EmployeeTypeEndpoint}/GetEmployeeTypeByid/{id}");
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeType model)
        {
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.EmployeeTypeEndpoint}/UpdateEmployeeType", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiServices.GetAsync<EmployeeTypeViewModel>($"{_apiSettings.EmployeeTypeEndpoint}/GetEmployeeTypeByid/{id}");
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.EmployeeTypeEndpoint}/DeleteEmployeeType/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }
}
