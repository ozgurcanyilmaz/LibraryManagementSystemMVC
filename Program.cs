using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

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

app.Use(async (context, next) =>
{
    Console.WriteLine($"Requested Path: {context.Request.Path}");
    await next();
});
// AccountController routes
app.MapControllerRoute(
    name: "account-login",
    pattern: "Account/StudentLogin",
    defaults: new { controller = "Account", action = "StudentLogin" }
);

app.MapControllerRoute(
    name: "account-register",
    pattern: "Account/Register",
    defaults: new { controller = "Account", action = "Register" }
);

app.MapControllerRoute(
    name: "account-admin-login",
    pattern: "Account/AdminLogin",
    defaults: new { controller = "Account", action = "AdminLogin" }
);

// AdminController routes
app.MapControllerRoute(
    name: "admin-portal",
    pattern: "Admin/Portal",
    defaults: new { controller = "Admin", action = "Portal" }
);

// BookController routes
app.MapControllerRoute(
    name: "book-add",
    pattern: "Book/Add/{id?}",
    defaults: new { controller = "Book", action = "Add" }
);

app.MapControllerRoute(
    name: "book-delete",
    pattern: "Book/Delete/{isbn?}",
    defaults: new { controller = "Book", action = "Delete" }
);

app.MapControllerRoute(
    name: "book-rented",
    pattern: "Book/Rented",
    defaults: new { controller = "Book", action = "Rented" }
);

app.MapControllerRoute(
    name: "book-update",
    pattern: "Book/Update/{isbn?}",
    defaults: new { controller = "Book", action = "Update" }
);

// StudentController routes
app.MapControllerRoute(
    name: "student-page",
    pattern: "Student/Page",
    defaults: new { controller = "Student", action = "Page" }
);

app.MapControllerRoute(
    name: "student-returnbook",
    pattern: "Student/ReturnBook",
    defaults: new { controller = "Student", action = "ReturnBook" }
);

// HomeController routes
app.MapControllerRoute(
    name: "home-index",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "home-privacy",
    pattern: "Home/Privacy",
    defaults: new { controller = "Home", action = "Privacy" }
);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Veritabanının mevcut olduğundan emin olun
    context.Database.EnsureCreated();

    // Admin kullanıcısını kontrol et ve gerekirse oluştur
    if (!context.Users.Any(u => u.Username == "admin"))
    {
        var adminUser = new User
        {
            Username = "admin",
            Password = BCrypt.Net.BCrypt.HashPassword("admin"), // Şifreyi hashle
            Role = "Admin" // Admin rolü
        };

        context.Users.Add(adminUser);
        context.SaveChanges();

        Console.WriteLine("Admin user created with username: admin and password: admin");
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}

app.Run();
