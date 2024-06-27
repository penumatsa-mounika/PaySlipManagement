﻿using Microsoft.AspNetCore.Mvc;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class EmployeeController : Controller
    {

        private APIServices _apiServices;
        public EmployeeController(APIServices apiService)
        {
            this._apiServices = apiService;
        }

        //GET: EmployeeController

        public async Task<IActionResult> Index()
        {
            TempData["message"] = "This is Department Index";

            var Departments = await _apiServices.GetAllAsync<PaySlipManagement.UI.Models.EmployeeViewModel>("api/Employee/GetAllEmployees");
            return View(Departments);
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

                // Make a POST request to the Web API
                var response = await _apiServices.PostAsync("api/Employee/CreateEmployee", model);

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
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.EmployeeViewModel>($"api/Employee/GetEmployeeById/{id}");
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Make a POST request to the Web API
                var response = await _apiServices.PutAsync<EmployeeViewModel>("/api/Employee/UpdateEmployee", model);

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
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.EmployeeViewModel>($"api/Employee/GetEmployeeById/{id}");
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _apiServices.GetAsync<EmployeeViewModel>($"api/Employee/GetEmployeeById/{id}");
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"api/Employee/DeleteEmployee/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }  
}




