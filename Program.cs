using Microsoft.EntityFrameworkCore;
using RinkuHRApp.Data;
using RinkuHRApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<HumanResourcesContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IPositionService, PositionService>();
builder.Services.AddTransient<IPayrollService, PayrollService>();
builder.Services.AddTransient<IPeriodService, PeriodService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=PayrollSelect}/{id?}");

app.Run();
