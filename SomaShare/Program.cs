using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;
using SomaShare.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity + cookie settings
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false; // for demo
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";       // redirect here if not logged in
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Business services
builder.Services.AddScoped<TextbookService>();
builder.Services.AddScoped<WantedAdService>();   // ✅ register WantedAdService
builder.Services.AddScoped<OfferService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ForumService>();
builder.Services.AddScoped<ChatService>();

// App language service (singleton, simple toggle)
builder.Services.AddSingleton<LanguageService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.Initialize(context, userManager, roleManager);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Public}/{id?}");

app.Run();
