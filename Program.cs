using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("LibraryAuth")
    .AddCookie("LibraryAuth", options =>
    {
        options.LoginPath = "/Account/StudentLogin";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRentalService, RentalService>();


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

app.MapControllerRoute(
    name: "student-rentbook",
    pattern: "Student/RentBook",
    defaults: new { controller = "Student", action = "RentBook" }
);

app.MapControllerRoute(
    name: "student-rentbookprocess",
    pattern: "Student/RentBookProcess",
    defaults: new { controller = "Student", action = "RentBookProcess" }
);

app.MapControllerRoute(
    name: "student-returnbookprocess",
    pattern: "Student/ReturnBookProcess",
    defaults: new { controller = "Student", action = "ReturnBookProcess" }
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

//ilk ba�ta bir admin olu�turmak i�in
/* using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

   // Veritaban�n�n mevcut oldu�undan emin olun
   context.Database.EnsureCreated();

   // Admin kullan�c�s�n� kontrol et ve gerekirse olu�tur
   if (!context.Users.Any(u => u.Username == "admin"))
   {
       var adminUser = new User
       {
           Username = "admin",
           Password = BCrypt.Net.BCrypt.HashPassword("admin"), // �ifreyi hashle
           Role = "Admin" // Admin rol�
       };

       context.Users.Add(adminUser);
       context.SaveChanges();

       Console.WriteLine("Admin user created with username: admin and password: admin");
   }
   else
   {
       Console.WriteLine("Admin user already exists.");
   }
} */

app.Run();
