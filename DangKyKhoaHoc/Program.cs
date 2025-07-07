using DangKyKhoaHoc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//----Đăng ký DbContext
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DangKyKhoaHocContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DangKyKhoaHocDb")));
//---Bổ sung Session
builder.Services.AddSession();
//---Bổ sung Authentication and Authorization
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
