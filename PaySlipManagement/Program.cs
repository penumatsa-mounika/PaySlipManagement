using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.BAL.Interfaces;
using PaySlipManagement.DAL.Implementations;
using NPOI.SS.Formula.Functions;
using PaySlipManagement.DAL.Interfaces;
using PaySlipManagement.API.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using Serilog.Events;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// Serilog setup with the connection string from the configuration
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();
Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

// Add services to the container.
builder.Services.AddDbContextFactory<LoggingDbContext>(options =>
        options.UseSqlServer("Server=localhost\\SQLEXPRESS01;database=PayslipManagement;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true"));
builder.Services.AddTransient<IExceptionLoggerService, ExceptionLoggerService>();

builder.Services.AddScoped<IEmployeeTypeBALRepo, EmployeeTypeBALRepo>();
builder.Services.AddScoped<ILeaveRequestsBALRepo, LeaveRequestsBALRepo>();
builder.Services.AddScoped<ILeavesBALRepo, LeavesBALRepo>();
builder.Services.AddScoped<IDepartmentBALRepo, DepartmentBALRepo>();
builder.Services.AddScoped<IEmployeeBALRepo,EmployeeBALRepo>();
builder.Services.AddScoped<IAccountDetailsBALRepo, AccountDetailsBALRepo>();
builder.Services.AddScoped<ICompanyDetailsBALRepo, CompanyDetailsBALRepo>();
builder.Services.AddScoped<ISalaryBALRepo, SalaryBALRepo>();
builder.Services.AddScoped<IUserRoleBALRepo, UserRoleBALRepo>();
builder.Services.AddScoped<IRoleBALRepo, RoleBALRepo>();
builder.Services.AddScoped<IHolidayImageBALRepo, HolidayImageBALRepo>();
builder.Services.AddScoped<IHolidayPdfBALRepo, HolidayPdfBALRepo>();
builder.Services.AddScoped<IPayslipDetailsBALRepo, PayslipDetailsBALRepo>();
builder.Services.AddScoped<ICTCDetailsBALRepo, CTCDetailsBALRepo>();
builder.Services.AddScoped<IManagerBALRepo, ManagerBALRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
