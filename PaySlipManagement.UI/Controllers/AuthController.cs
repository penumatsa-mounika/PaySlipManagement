using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaySlipManagement.Common.Models;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using PaySlipManagement.UI.Models.DTO;
using Microsoft.Extensions.Options;

namespace PaySlipManagement.UI.Controllers
{
    public class AuthController : Controller
    {
        private APIServices _apiServices;
        private readonly ApiSettings _apiSettings;
        public AuthController(APIServices apiServices, IOptions<ApiSettings> apiSettings)
        {
            this._apiServices = apiServices;
            _apiSettings = apiSettings.Value;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Emp_Code = model.Emp_Code,
                    Password = model.Password,
                    Role = ""
                };

                var response = await _apiServices.PostAsync($"{_apiSettings.UserEndpoint}/Login", user);
                if (!string.IsNullOrEmpty(response))
                {
                    var jsonresponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                    var token = jsonresponse.Token;

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    Response.Cookies.Append("empCode", jsonresponse.User.EmpCode, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    var claimsPrincipal = GetClaimsPrincipalFromToken(token);
                    var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                    //if (roleClaim != null)
                    //{
                    //    var role = roleClaim.Value;
                    //    TempData["empcode"] = jsonresponse.User.EmpCode;

                    //    if (role == "Admin")
                    //    {
                    //        TempData["IsAdminRole"] = true;
                    //        return RedirectToAction("Index", "Employee");
                    //    }
                    //    else if (role == "Employee")
                    //    {
                    //        TempData["IsEmployeeRole"] = true;
                    //        return RedirectToAction("GeneratePdf", "Employee");
                    //    }
                    //}
                    if (roleClaim != null)
                    {
                        var role = roleClaim.Value;
                        TempData["empcode"] = jsonresponse.User.EmpCode;

                        // Set role and token in cookies
                        SetCookie("AuthToken", token);
                        SetCookie("empCode", jsonresponse.User.EmpCode);
                        HttpContext.Session.SetString("empCode", jsonresponse.User.EmpCode);
                        SetCookie("UserRole", role);

                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Employee");
                        }
                        else if (role == "Employee")
                        {
                            return RedirectToAction("GeneratePdf", "Employee");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid role");
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed or login was unsuccessful");
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("empCode");
            return RedirectToAction("Login", "Auth");
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key_here_1234567890_1234567890_1234567890_");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            return principal;
        }
        private void SetCookie(string key, string value)
        {
            Response.Cookies.Append(key, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
