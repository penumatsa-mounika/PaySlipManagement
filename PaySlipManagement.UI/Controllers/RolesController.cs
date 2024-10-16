﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace PaySlipManagement.UI.Controllers
{
    public class RolesController : Controller
    {
        private readonly APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public RolesController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            _apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }


        // GET: Roles
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            TempData["message"] = "This is Role Index";

            // Fetch all roles
            var roles = await _apiServices.GetAllAsync<RolesViewModel>($"{_apiSettings.RolesEndpoint}/GetAllAsyncRoles");

            // Calculate total number of items
            int totalItems = roles.Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Ensure current page is within bounds
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            // Calculate the number of items to skip
            int skipItems = (currentPage - 1) * pageSize;

            // Get the paginated data for the current page
            var pagedRoles = roles.Skip(skipItems).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedRoles);
        }

        // GET: Roles/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiServices.PostAsync($"{_apiSettings.RolesEndpoint}/CreateRoles", model);
                if (!string.IsNullOrEmpty(response) && response == "Role Registered Successfully" || response == "true")
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                      //Handle the case where the API request fails or register is unsuccessful
                      if (response != null)
                      {
                          ModelState.AddModelError(string.Empty, response);
                      }
                      ModelState.AddModelError(string.Empty, "API request failed or Create was unsuccessful");
                }
            }
            //return View(model);
            
            ModelState.AddModelError(string.Empty, "Invalid Create attempt");
            return View();
        }

       
        // GET: Roles/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.RolesViewModel>($"{_apiSettings.RolesEndpoint}/GetByIdAsyncRoles/{id}");
            return View(data);

        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,RolesViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    // Make a POST request to the Web API
            //    var response = await _apiServices.PutAsync($"{_apiSettings.RolesEndpoint}/UpdateRoles", role);

            //    if (!string.IsNullOrEmpty(response) && response == "Role is Updated Successfully" || response == "true")
            //    {
            //        TempData["message"] = response;
            //        // Handle a successful Updated
            //        return RedirectToAction("Index");

            //    }
            //    else
            //    {
            //        // Handle the case where the API request fails or register is unsuccessful
            //        if (response != null)
            //        {
            //            ModelState.AddModelError(string.Empty, response);
            //        }
            //        ModelState.AddModelError(string.Empty, "API request failed or Update was unsuccessful");
            //    }

            //}
            //   ModelState.AddModelError(string.Empty, "Invalid Update attempt");
            //   return View("Update");
            if (ModelState.IsValid)
            {
                await _apiServices.PutAsync($"{_apiSettings.RolesEndpoint}/UpdateRoles", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.RolesViewModel>($"{_apiSettings.RolesEndpoint}/GetByIdAsyncRoles/{id}");
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _apiServices.GetAsync<PaySlipManagement.UI.Models.RolesViewModel>($"{_apiSettings.RolesEndpoint}/GetByIdAsyncRoles/{id}");
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiServices.GetAsync<bool>($"{_apiSettings.RolesEndpoint}/DeleteRoles/{id}");
            if (response == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Delete");
        }
    }
}
   

