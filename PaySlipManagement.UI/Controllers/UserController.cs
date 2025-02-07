﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using NuGet.Configuration;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;

namespace PaySlipManagement.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly APIServices _apiService;
        private readonly ApiSettings _apiSettings;
        public UserController(APIServices apiService, IOptions<ApiSettings> apiSettings)
        {
            _apiService = apiService;
            _apiSettings = apiSettings.Value;
        }
        // GET: UserController
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            ViewData["ToastMessage"] = "Retrieved all Users.";

            // Fetch all users from the API
            var userList = await _apiService.GetAllAsync<UsersViewModel>($"{_apiSettings.UserEndpoint}/GetAllUsers");

            // Calculate total number of items
            int totalItems = userList.Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Ensure the current page is within bounds
            int currentPage = page > totalPages ? totalPages : page;
            currentPage = currentPage < 1 ? 1 : currentPage;

            // Calculate the number of items to skip
            int skipItems = (currentPage - 1) * pageSize;

            // Get the paginated data for the current page
            var pagedUserList = userList.Skip(skipItems).Take(pageSize).ToList();

            // Pass pagination data to the view using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedUserList);
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var data = await _apiService.GetAsync<UsersViewModel>($"{_apiSettings.UserEndpoint}/GetUserById/{id}");
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // GET: UserController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersViewModel user)
        {
            if (ModelState.IsValid)
            {
                Users u = new Users();
                u.Id = user.Id;
                u.Emp_Code = user.Emp_Code;
                u.Password = user.Password;
                u.Email = user.Email;
                //u.Role = user.Role;
                var data = await _apiService.PostAsync<Users, bool>($"{_apiSettings.UserEndpoint}/Register", u);
                if (data !=null && data==true)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: UserController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _apiService.GetAsync<UsersViewModel>($"{_apiSettings.UserEndpoint}/GetUserById/{id}");
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsersViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Users u = new Users();
                u.Id = user.Id;
                u.Emp_Code = user.Emp_Code;
                u.Password = user.Password;
                u.Email = user.Email;
                //u.Role = user.Role;
                var existinguser= await _apiService.PutAsync<Users>($"{_apiSettings.UserEndpoint}/UpdateUser", u);
                if (existinguser == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var users = await _apiService.GetAsync<UsersViewModel>($"{_apiSettings.UserEndpoint}/GetUserById/{id}");
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: UsersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserConfirmed(int id)
        {
            var users = await _apiService.GetAsync<bool>($"{_apiSettings.UserEndpoint}/DeleteUser/{id}");
            if (users != null && users == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
