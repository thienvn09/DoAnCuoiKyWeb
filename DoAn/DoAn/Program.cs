
using DoAn.Models.Momo;
using DoAn.Service;
using DoAn.Service.Vnpay;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
//Đăng ký DbContextz
//Momo
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

//Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();


// Add services to the container.
// Đăng ký các DAL vào DI container
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Customer/SignIn";
        //truy cập trang có yêu cầu đăng nhập
        //mà người dùng chưa đăng nhập
        //thì sẽ tự động chuyển sang đường dẫn được khai báo trong LoginPath
        option.AccessDeniedPath = "/AccessDenied";
        // Khi người dùng truy cập trang 
        // mà người dùng đó không được quyền truy cập
        // thì sẽ tự động chuyển sang đường dẫn được khai báo trong AccessDeniedPath
    });

// Use Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=home}/{action=Index}/{id?}");
// nằm trước MapControllerRoute có name là default

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
