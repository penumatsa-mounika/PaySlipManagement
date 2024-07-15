using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.UI.Common;
using PaySlipManagement.UI.Models;
using System.Text;
using PaySlipManagement.Common.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<APIServices>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});
builder.Services.AddScoped<IDepartmentBALRepo, DepartmentBALRepo>();
builder.Services.AddScoped<IEmployeeBALRepo, EmployeeBALRepo>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});


var key = Encoding.ASCII.GetBytes("your_secret_key_here_1234567890_1234567890_1234567890_");
  builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; }).
    AddJwtBearer(x => { x.RequireHttpsMetadata = false; x.SaveToken = true; x.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = false, ValidateAudience = false }; });
builder.Services.AddAuthorization(options => { options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin")); options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Employee")); });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<RoleMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
