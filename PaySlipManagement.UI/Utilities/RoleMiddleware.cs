using Microsoft.AspNetCore.Http;

namespace PaySlipManagement.Common.Utilities
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var httpContext = _httpContextAccessor.HttpContext;

            //if (httpContext != null)
            //{
            //    // Delete cookies
            //    httpContext.Response.Cookies.Delete("AuthToken");
            //    httpContext.Response.Cookies.Delete("UserRole");
            //    httpContext.Response.Cookies.Delete("empCode");
            //}
            var empCode = context.Session.GetString("empCode");


            var role = context.Request.Cookies["UserRole"];
            var manager = context.Request.Cookies["ManagerCode"];
            if (empCode == manager)
            {
                context.Items["IsManager"] = true;
            }
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin")
                {
                    context.Items["IsAdminRole"] = true;
                }
                else if (role == "Employee")
                {
                    context.Items["IsEmployeeRole"] = true;
                }
            }

            await _next(context);
        }
    }
}
