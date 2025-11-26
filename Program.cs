using family_tree_builder.Data;
using Microsoft.AspNetCore.Authentication.Cookies;  
using Microsoft.AspNetCore.Authentication.Google;   
using Microsoft.AspNetCore.Identity;                  
using Microsoft.EntityFrameworkCore;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();  

// Database - SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=familytree.db"));


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.SignInScheme = IdentityConstants.ExternalScheme;   
    });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});
var senderEmail =  builder.Configuration["Email:Sender"] 
                  ?? throw new InvalidOperationException("Email:Sender not configured");       
var appPassword = builder.Configuration["Email:AppPassword"] 
                  ?? throw new InvalidOperationException("Email:AppPassword not configured");

builder.Services.AddFluentEmail(senderEmail)
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient("smtp.gmail.com")
    {
        UseDefaultCredentials = false,
        Port = 587,
        Credentials = new NetworkCredential(senderEmail, appPassword),
        EnableSsl = true
    });
    
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmailService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

// Cookies consent
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Require consent for non-essential cookies
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

var app = builder.Build();

app.UseCookiePolicy();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();   

app.Run();