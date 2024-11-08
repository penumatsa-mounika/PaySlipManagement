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
using Hangfire;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<APIServices>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});
builder.Services.AddScoped<IDepartmentBALRepo, DepartmentBALRepo>();
builder.Services.AddScoped<IEmployeeBALRepo, EmployeeBALRepo>();
builder.Services.AddScoped<ILeavesBALRepo, LeavesBALRepo>(); //register 
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


// Add Hangfire services for background jobs
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromSeconds(15),
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            });

});

// Add the Hangfire server
builder.Services.AddHangfireServer();

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



var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

//recurringJobManager.AddOrUpdate(
//    "carry-forward-leaves",
//    Job.FromExpression<ILeavesBALRepo>(repo => repo.CarryForwardLeavesAsync()),
//    Cron.Yearly(12, 31, 23, 59) // Runs at 23:59 on Dec 31 every year

//);

// Schedule the carry forward job to run at midnight on December 31st every year
RecurringJob.AddOrUpdate<ILeavesBALRepo>(
    "carry-forward-leaves",
    repo => repo.CarryForwardLeavesAsync(),
    "59 23 31 12 *",  // Runs once annually on December 31st at 11:59 PM
    new RecurringJobOptions() { }
);

// Schedule the monthly leave addition job to run at midnight on the first of every month
RecurringJob.AddOrUpdate<ILeavesBALRepo>(
    "monthly-leave-addition",
    repo => repo.MonthlyLeaveAdditionAsync(),
    "0 0 1 * *",  // Runs monthly on the 1st at 12:00 AM
    new RecurringJobOptions() { }
);





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<RoleMiddleware>();

app.UseHangfireDashboard("/hangfire");
app.UseMiddleware<RoleMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
