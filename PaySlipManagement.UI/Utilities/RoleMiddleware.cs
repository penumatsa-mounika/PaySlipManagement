namespace PaySlipManagement.Common.Utilities
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var empCode = context.Session.GetString("empCode");


            var role = context.Request.Cookies["UserRole"];

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
