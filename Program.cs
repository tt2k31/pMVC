using App.Models;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//đăng kí dùng trang razor
// builder.Services.Configure<RazorViewEngineOptions>(options => {
//      options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);
// });

//đăng kí db
builder.Services.AddDbContext<AppDbContext>(option =>{
    option.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString"));
});

//đănkí dịch vụ email
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings")
);

builder.Services.AddSingleton<IEmailSender, SendMailService>();

// Đăng kí DbContext
builder.Services.AddDbContext<AppDbContext>(options => 
       options.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString")));

//Đăng kí Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(option =>{
    option.LoginPath = "/Login";
    option.LogoutPath = "/Logout";
    option.AccessDeniedPath = "/AccessDenied";
    // option.LoginPath = "/Login/";
    // option.LogoutPath = "/Logout/";
    // option.AccessDeniedPath = "/kotruycap.html";
});

//tùy biến Identity
// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds (10); // Khóa ...
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;          // Cần xác để đăng nhập

});

builder.Services.AddAuthentication()
                    .AddGoogle(option =>{
                        var gconfic = builder.Configuration.GetSection("Authentication:Google");
                        option.ClientId = gconfic["ClientId"];
                        option.ClientSecret =gconfic["ClientSecret"];
                        //mặc định callback là: /sigin-google 
                        option.CallbackPath = "/LoginByGoogle";
                    })
                    .AddFacebook(option =>{
                        var fconfic = builder.Configuration.GetSection("Authentication:FaceBook");
                        option.AppId = fconfic["AppId"];
                        option.AppSecret =fconfic["AppSecret"];
                        
                        option.CallbackPath = "/LoginByFaceBook";
                    });

// Thay đổi hiển thị lỗi
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

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
app.UseAuthentication();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "product",
//     pattern: "{controller=Home}/{action=Index}/{id?}",
//     areaName: "ProductManage"
// );
app.Run();
