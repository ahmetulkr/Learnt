using LearntMVCProject.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi
    options.Cookie.HttpOnly = true;                // Tarayıcıdan erişim kısıtlaması
    options.Cookie.IsEssential = true;             // GDPR uyumu
});

var app = builder.Build();

// Veritabanı tablolarını oluştur
try
{
    string connectionString = "Host=localhost;Port=5432;Database=UserDb;Username=postgres;Password=3302031";
    var dbInitializer = new DatabaseInitializer(connectionString);
    dbInitializer.Initialize();
    Console.WriteLine("Veritabanı başarıyla hazırlandı.");
}
catch (Exception ex)
{
    Console.WriteLine($"Veritabanı hazırlanırken hata oluştu: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");
    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();
